namespace TransportControl.Core.Entities;

/// <summary>
/// Entidad principal que representa un viaje en el sistema de transporte
/// </summary>
public class Trip : BaseEntity
{
    
    // Información básica del viaje
    public int OriginId { get; set; }
    public Place Origin { get; set; } = null!;
    
    public int DestinationId { get; set; }
    public Place Destination { get; set; } = null!;
    
    // Fechas y horas programadas
    public DateTime ScheduledStartDateTime { get; set; }
    public DateTime ScheduledEndDateTime { get; set; }
    
    // Operador asignado
    public int OperatorId { get; set; }
    public Operator Operator { get; set; } = null!;
    
    // Campos adicionales para procesos futuros
    public DateTime? ActualStartDateTime { get; set; }
    public DateTime? ActualEndDateTime { get; set; }
    public TripStatus Status { get; set; } = TripStatus.Scheduled;
    public decimal? EstimatedDistance { get; set; }
    public decimal? ActualDistance { get; set; }
    public string? Notes { get; set; }
    public string? VehicleId { get; set; }
}

/// <summary>
/// Estados posibles de un viaje
/// </summary>
public enum TripStatus
{
    Scheduled = 1,      // Programado
    InProgress = 2,     // En progreso
    Completed = 3,      // Completado
    Cancelled = 4,      // Cancelado
    Delayed = 5         // Retrasado
}