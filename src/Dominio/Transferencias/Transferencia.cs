using Dominio.Abstracciones;
using Dominio.Compartido;
using Dominio.Cuentas;
using Dominio.Transferencias.Events;

namespace Dominio.Transferencias;

public sealed class Transferencia : Entidad
{
    private Transferencia(
        Guid cuentaOrigenId,
        Guid cuentaDestinoId,
        Dinero monto,
        EstadoTransferencia estado,
        DateTime creadoEnUtc
    )
    {
        CuentaOrigenId = cuentaOrigenId;
        CuentaDestinoId = cuentaDestinoId;
        Monto = monto;
        Estado = estado;
        CreadoEnUtc = creadoEnUtc;
    }

    public Guid CuentaOrigenId { get; private set; }
    public Guid CuentaDestinoId { get; private set; }
    public Dinero Monto { get; private set; }
    public EstadoTransferencia Estado { get; private set; }
    public DateTime CreadoEnUtc { get; private set; }
    public DateTime? ProcesadoEnUtc { get; private set; }

    public static Resultado<Transferencia> Iniciar(
        Cuenta cuentaOrigen,
        Cuenta cuentaDestino,
        Dinero monto,
        DateTime utcNow
    )
    {
        if (cuentaOrigen.Estado != EstadoCuenta.Activa)
            return Resultado.Fallo<Transferencia>(ErroresTransferencia.CuentaOrigenInvalida);

        if (cuentaDestino.Estado != EstadoCuenta.Activa)
            return Resultado.Fallo<Transferencia>(ErroresTransferencia.CuentaDestinoInvalida);

        if (monto.Monto <= 0)
            return Resultado.Fallo<Transferencia>(ErroresTransferencia.MontoInvalido);

        var transferencia = new Transferencia(
            cuentaOrigen.Id,
            cuentaDestino.Id,
            monto,
            EstadoTransferencia.Pendiente,
            utcNow
        );

        return Resultado.Exito(transferencia);
    }

    public Resultado Ejecutar(Cuenta cuentaOrigen, Cuenta cuentaDestino, DateTime utcNow)
    {
        var resultadoDebito = cuentaOrigen.Debitar(Monto);

        if (resultadoDebito.EsFallo)
        {
            Estado = EstadoTransferencia.Fallida;
            ProcesadoEnUtc = utcNow;
            RegistrarEventoDominio(
                new TransferenciaFallidaEventoDominio(Id, resultadoDebito.Error.Nombre)
            );

            return Resultado.Fallo(resultadoDebito.Error);
        }

        cuentaDestino.Acreditar(Monto);

        Estado = EstadoTransferencia.Completada;
        ProcesadoEnUtc = utcNow;

        RegistrarEventoDominio(new TransferenciaCompletadaEventoDominio(Id));

        return Resultado.Exito();
    }
}
