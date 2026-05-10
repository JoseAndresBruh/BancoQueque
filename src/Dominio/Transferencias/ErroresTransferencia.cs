using Dominio.Abstracciones;

namespace Dominio.Transferencias;

public static class ErroresTransferencia
{
    public static readonly Error CuentaOrigenInvalida = new(
        "Transferencia.CuentaOrigenInvalida",
        "La cuenta de origen no está activa."
    );

    public static readonly Error CuentaDestinoInvalida = new(
        "Transferencia.CuentaDestinoInvalida",
        "La cuenta de destino no está activa."
    );

    public static readonly Error MontoInvalido = new(
        "Transferencia.MontoInvalido",
        "El monto a transferir debe ser mayor a cero."
    );
}
