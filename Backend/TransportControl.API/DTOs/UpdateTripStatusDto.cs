using TransportControl.Core.Entities;

namespace TransportControl.API.DTOs;

/// <summary>
/// DTO para actualizar solo el estado de un viaje
/// </summary>
public class UpdateTripStatusDto
{
    /// <summary>
    /// Nuevo estado del viaje
    /// </summary>
    public TripStatus Status { get; set; }
    
    /// <summary>
    /// Fecha y hora actual de inicio (opcional, se usa cuando el estado cambia a En Progreso)
    /// </summary>
    public DateTime? ActualStartDateTime { get; set; }
    
    /// <summary>
    /// Fecha y hora actual de fin (opcional, se usa cuando el estado cambia a Completado)
    /// </summary>
    public DateTime? ActualEndDateTime { get; set; }
    
    /// <summary>
    /// Notas adicionales sobre el cambio de estado
    /// </summary>
    public string? Notes { get; set; }
}