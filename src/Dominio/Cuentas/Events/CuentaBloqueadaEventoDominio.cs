using Dominio.Abstracciones;

namespace Dominio.Cuentas.Events;

public record CuentaBloqueadaEventoDominio(Guid CuentaId) : IEventoDominio;
