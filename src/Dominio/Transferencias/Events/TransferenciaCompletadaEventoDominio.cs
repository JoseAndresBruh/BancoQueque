using Dominio.Abstracciones;

namespace Dominio.Transferencias.Events;

public record TransferenciaCompletadaEventoDominio(Guid TransferenciaId) : IEventoDominio;
