using Dominio.Abstracciones;
using Dominio.Compartido;
using Dominio.Cuentas.Events;

namespace Dominio.Cuentas;

public sealed class Cuenta : Entidad
{
    private Cuenta(
        Guid clienteId,
        string numeroCuenta,
        Dinero saldo,
        EstadoCuenta estado,
        DateTime creadoEnUtc
    )
    {
        ClienteId = clienteId;
        NumeroCuenta = numeroCuenta;
        Saldo = saldo;
        Estado = estado;
        CreadoEnUtc = creadoEnUtc;
    }

    public Guid ClienteId { get; private set; }
    public string NumeroCuenta { get; private set; }
    public Dinero Saldo { get; private set; }
    public EstadoCuenta Estado { get; private set; }
    public DateTime CreadoEnUtc { get; private set; }
    public DateTime? ActualizadoEnUtc { get; private set; }

    public static Cuenta Abrir(Guid clienteId, string numeroCuenta, DateTime utcNow)
    {
        // Forzamos que toda cuenta nazca en USD para nuestra banca nacional
        var cuenta = new Cuenta(
            clienteId,
            numeroCuenta,
            Dinero.Cero(Moneda.Usd),
            EstadoCuenta.Activa,
            utcNow
        );

        cuenta.RegistrarEventoDominio(new CuentaAbiertaEventoDominio(cuenta.Id));

        return cuenta;
    }

    public Resultado Bloquear(DateTime utcNow)
    {
        if (Estado == EstadoCuenta.Bloqueada)
        {
            return Resultado.Fallo(ErroresCuenta.YaBloqueada);
        }

        Estado = EstadoCuenta.Bloqueada;
        ActualizadoEnUtc = utcNow;

        RegistrarEventoDominio(new CuentaBloqueadaEventoDominio(Id));

        return Resultado.Exito();
    }

    internal void Acreditar(Dinero monto)
    {
        Saldo = Saldo + monto;
    }

    internal Resultado Debitar(Dinero monto)
    {
        if (Saldo.Monto < monto.Monto)
        {
            return Resultado.Fallo(ErroresCuenta.FondosInsuficientes);
        }

        Saldo = Saldo - monto;
        return Resultado.Exito();
    }
}
