using TransportControl.API.DTOs;
using TransportControl.Core.Entities;

namespace TransportControl.API.Mappings;

/// <summary>
/// Extensiones de mapeo para la entidad Trip
/// </summary>
public static class TripMappingExtensions
{
    /// <summary>
    /// Convierte una entidad Trip a TripResponseDto
    /// </summary>
    /// <param name="trip">Entidad Trip</param>
    /// <returns>DTO de respuesta</returns>
    public static TripResponseDto ToResponseDto(this Trip trip)
    {
        return new TripResponseDto
        {
            Id = trip.Id,
            OriginId = trip.OriginId,
            OriginName = trip.Origin?.Name ?? string.Empty,
            Origin = trip.Origin?.ToResponseDto() ?? new PlaceResponseDto(),
            DestinationId = trip.DestinationId,
            DestinationName = trip.Destination?.Name ?? string.Empty,
            Destination = trip.Destination?.ToResponseDto() ?? new PlaceResponseDto(),
            OperatorId = trip.OperatorId,
            OperatorName = trip.Operator?.FullName ?? string.Empty,
            Operator = trip.Operator?.ToResponseDto() ?? new OperatorResponseDto(),
            ScheduledStartDateTime = trip.ScheduledStartDateTime,
            ScheduledEndDateTime = trip.ScheduledEndDateTime,
            ActualStartDateTime = trip.ActualStartDateTime,
            ActualEndDateTime = trip.ActualEndDateTime,
            Status = (int)trip.Status,
            EstimatedDistance = trip.EstimatedDistance,
            ActualDistance = trip.ActualDistance,
            Notes = trip.Notes,
            VehicleId = trip.VehicleId,
            CreatedAt = trip.CreatedAt,
            ModifiedAt = trip.ModifiedAt
        };
    }

    /// <summary>
    /// Convierte una colección de entidades Trip a TripResponseDto
    /// </summary>
    /// <param name="trips">Colección de entidades Trip</param>
    /// <returns>Colección de DTOs de respuesta</returns>
    public static IEnumerable<TripResponseDto> ToResponseDto(this IEnumerable<Trip> trips)
    {
        return trips.Select(t => t.ToResponseDto());
    }
}
