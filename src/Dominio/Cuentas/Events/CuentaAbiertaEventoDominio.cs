using Dominio.Abstracciones;

namespace Dominio.Cuentas.Events;

public record CuentaAbiertaEventoDominio(Guid CuentaId) : IEventoDominio;
