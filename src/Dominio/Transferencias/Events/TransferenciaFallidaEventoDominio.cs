using Dominio.Abstracciones;

namespace Dominio.Transferencias.Events;

public record TransferenciaFallidaEventoDominio(Guid TransferenciaId, string Razon)
    : IEventoDominio;
