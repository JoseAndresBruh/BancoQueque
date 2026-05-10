namespace Dominio.Compartido;

public record Dinero(decimal Monto, Moneda Moneda)
{
    public static Dinero operator +(Dinero primero, Dinero segundo)
    {
        if (primero.Moneda != segundo.Moneda)
        {
            throw new InvalidOperationException("Las monedas deben ser iguales");
        }

        return new Dinero(primero.Monto + segundo.Monto, primero.Moneda);
    }

    public static Dinero operator -(Dinero primero, Dinero segundo)
    {
        if (primero.Moneda != segundo.Moneda)
        {
            throw new InvalidOperationException("Las monedas deben ser iguales");
        }

        return new Dinero(primero.Monto - segundo.Monto, primero.Moneda);
    }

    public static Dinero Cero() => new(0, Moneda.Ninguno);

    public static Dinero Cero(Moneda moneda) => new(0, moneda);

    public bool EsCero() => this == Cero(Moneda);
}
