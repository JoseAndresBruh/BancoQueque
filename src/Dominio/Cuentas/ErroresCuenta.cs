using Dominio.Abstracciones;

namespace Dominio.Cuentas;

public static class ErroresCuenta
{
    public static readonly Error Inactiva = new(
        "Cuenta.Inactiva",
        "La cuenta no está activa para realizar operaciones."
    );

    public static readonly Error FondosInsuficientes = new(
        "Cuenta.FondosInsuficientes",
        "El saldo de la cuenta es insuficiente para esta transacción."
    );

    public static readonly Error YaBloqueada = new(
        "Cuenta.YaBloqueada",
        "La cuenta ya se encuentra bloqueada."
    );
}
