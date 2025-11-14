using TransportControl.API.DTOs;
using TransportControl.Core.Entities;

namespace TransportControl.API.Mappings;

/// <summary>
/// Extensiones de mapeo para la entidad Place
/// </summary>
public static class PlaceMappingExtensions
{
    /// <summary>
    /// Convierte una entidad Place a PlaceResponseDto
    /// </summary>
    /// <param name="place">Entidad Place</param>
    /// <returns>DTO de respuesta</returns>
    public static PlaceResponseDto ToResponseDto(this Place place)
    {
        return new PlaceResponseDto
        {
            Id = place.Id,
            Name = place.Name,
            Code = place.Code,
            Description = place.Description,
            Address = place.Address,
            City = place.City,
            State = place.State,
            Country = place.Country,
            PostalCode = place.PostalCode,
            Latitude = place.Latitude,
            Longitude = place.Longitude,
            Type = (int)place.Type,
            Status = (int)place.Status,
            IsOriginAllowed = place.IsOriginAllowed,
            IsDestinationAllowed = place.IsDestinationAllowed,
            ContactPerson = place.ContactPerson,
            ContactPhone = place.ContactPhone,
            ContactEmail = place.ContactEmail,
            OperatingHoursStart = place.OperatingHoursStart,
            OperatingHoursEnd = place.OperatingHoursEnd,
            SpecialInstructions = place.SpecialInstructions,
            CreatedAt = place.CreatedAt,
            ModifiedAt = place.ModifiedAt
        };
    }

    /// <summary>
    /// Convierte una colección de entidades Place a PlaceResponseDto
    /// </summary>
    /// <param name="places">Colección de entidades Place</param>
    /// <returns>Colección de DTOs de respuesta</returns>
    public static IEnumerable<PlaceResponseDto> ToResponseDto(this IEnumerable<Place> places)
    {
        return places.Select(p => p.ToResponseDto());
    }
}
