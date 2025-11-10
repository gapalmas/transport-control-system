using System.ComponentModel.DataAnnotations;

namespace TransportControl.API.DTOs;

/// <summary>
/// DTO para crear un nuevo viaje
/// </summary>
public class CreateTripDto
{
    [Required(ErrorMessage = "El ID del origen es requerido")]
    public int OriginId { get; set; }

    [Required(ErrorMessage = "El ID del destino es requerido")]
    public int DestinationId { get; set; }

    [Required(ErrorMessage = "El ID del operador es requerido")]
    public int OperatorId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio programada es requerida")]
    public DateTime ScheduledStartDateTime { get; set; }

    [Required(ErrorMessage = "La fecha de fin programada es requerida")]
    public DateTime ScheduledEndDateTime { get; set; }

    public DateTime? ActualStartDateTime { get; set; }
    public DateTime? ActualEndDateTime { get; set; }
    public int Status { get; set; } = 1; // Scheduled por defecto
    public decimal? EstimatedDistance { get; set; }
    public decimal? ActualDistance { get; set; }
    public string? Notes { get; set; }
    public string? VehicleId { get; set; }
}