using Microsoft.AspNetCore.Mvc;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.API.DTOs;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de viajes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly ILogger<TripsController> _logger;

    public TripsController(ITripService tripService, ILogger<TripsController> logger)
    {
        _tripService = tripService;
        _logger = logger;
    }

    /// <summary>
    /// Convierte una entidad Trip a TripResponseDto
    /// </summary>
    /// <param name="trip">Entidad Trip</param>
    /// <returns>DTO de respuesta</returns>
    private static TripResponseDto MapToTripResponseDto(Trip trip)
    {
        return new TripResponseDto
        {
            Id = trip.Id,
            OriginId = trip.OriginId,
            OriginName = trip.Origin?.Name ?? string.Empty,
            Origin = trip.Origin != null ? MapToPlaceResponseDto(trip.Origin) : new PlaceResponseDto(),
            DestinationId = trip.DestinationId,
            DestinationName = trip.Destination?.Name ?? string.Empty,
            Destination = trip.Destination != null ? MapToPlaceResponseDto(trip.Destination) : new PlaceResponseDto(),
            OperatorId = trip.OperatorId,
            OperatorName = trip.Operator?.FullName ?? string.Empty,
            Operator = trip.Operator != null ? MapToOperatorResponseDto(trip.Operator) : new OperatorResponseDto(),
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
    /// Convierte una entidad Place a PlaceResponseDto
    /// </summary>
    /// <param name="place">Entidad Place</param>
    /// <returns>DTO de respuesta</returns>
    private static PlaceResponseDto MapToPlaceResponseDto(Place place)
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
    /// Convierte una entidad Operator a OperatorResponseDto
    /// </summary>
    /// <param name="operatorEntity">Entidad Operator</param>
    /// <returns>DTO de respuesta</returns>
    private static OperatorResponseDto MapToOperatorResponseDto(Operator operatorEntity)
    {
        return new OperatorResponseDto
        {
            Id = operatorEntity.Id,
            FirstName = operatorEntity.FirstName,
            LastName = operatorEntity.LastName,
            FullName = operatorEntity.FullName,
            Email = operatorEntity.Email,
            Phone = operatorEntity.Phone,
            EmployeeId = operatorEntity.EmployeeId,
            LicenseNumber = operatorEntity.LicenseNumber,
            LicenseExpiryDate = operatorEntity.LicenseExpiryDate,
            Status = (int)operatorEntity.Status,
            DateOfBirth = operatorEntity.DateOfBirth,
            HireDate = operatorEntity.HireDate,
            Address = operatorEntity.Address,
            EmergencyContact = operatorEntity.EmergencyContact,
            EmergencyPhone = operatorEntity.EmergencyPhone,
            CreatedAt = operatorEntity.CreatedAt,
            ModifiedAt = operatorEntity.ModifiedAt
        };
    }

    /// <summary>
    /// Obtiene todos los viajes con paginación
    /// </summary>
    /// <param name="page">Número de página (por defecto 1)</param>
    /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
    /// <returns>Lista paginada de viajes</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripResponseDto>>> GetTrips(int page = 1, int pageSize = 10)
    {
        try
        {
            var trips = await _tripService.GetTripsAsync(page, pageSize);
            
            var tripDtos = trips.Select(MapToTripResponseDto);

            return Ok(tripDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los viajes");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un viaje específico por ID
    /// </summary>
    /// <param name="id">ID del viaje</param>
    /// <returns>Viaje encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TripResponseDto>> GetTrip(int id)
    {
        try
        {
            var trip = await _tripService.GetTripByIdAsync(id);

            if (trip == null)
            {
                return NotFound($"Viaje con ID {id} no encontrado");
            }

            var tripDto = MapToTripResponseDto(trip);

            return Ok(tripDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el viaje con ID {TripId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo viaje
    /// </summary>
    /// <param name="createTripDto">Datos del viaje a crear</param>
    /// <returns>Viaje creado</returns>
    [HttpPost]
    public async Task<ActionResult<TripResponseDto>> CreateTrip(CreateTripDto createTripDto)
    {
        try
        {
            // Crear la entidad Trip desde el DTO
            var trip = new Trip
            {
                OriginId = createTripDto.OriginId,
                DestinationId = createTripDto.DestinationId,
                OperatorId = createTripDto.OperatorId,
                ScheduledStartDateTime = createTripDto.ScheduledStartDateTime,
                ScheduledEndDateTime = createTripDto.ScheduledEndDateTime,
                ActualStartDateTime = createTripDto.ActualStartDateTime,
                ActualEndDateTime = createTripDto.ActualEndDateTime,
                Status = (TripStatus)createTripDto.Status,
                EstimatedDistance = createTripDto.EstimatedDistance,
                ActualDistance = createTripDto.ActualDistance,
                Notes = createTripDto.Notes,
                VehicleId = createTripDto.VehicleId
            };

            var createdTrip = await _tripService.CreateTripAsync(trip);
            var tripResponse = MapToTripResponseDto(createdTrip);

            return CreatedAtAction(nameof(GetTrip), new { id = createdTrip.Id }, tripResponse);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear viaje");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el viaje");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un viaje existente
    /// </summary>
    /// <param name="id">ID del viaje a actualizar</param>
    /// <param name="updateTripDto">Datos actualizados del viaje</param>
    /// <returns>Viaje actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<TripResponseDto>> UpdateTrip(int id, UpdateTripDto updateTripDto)
    {
        try
        {
            // Crear la entidad Trip con los datos actualizados
            var trip = new Trip
            {
                Id = id,
                OriginId = updateTripDto.OriginId,
                DestinationId = updateTripDto.DestinationId,
                OperatorId = updateTripDto.OperatorId,
                ScheduledStartDateTime = updateTripDto.ScheduledStartDateTime,
                ScheduledEndDateTime = updateTripDto.ScheduledEndDateTime,
                Status = updateTripDto.Status,
                ActualStartDateTime = updateTripDto.ActualStartDateTime,
                ActualEndDateTime = updateTripDto.ActualEndDateTime,
                EstimatedDistance = updateTripDto.EstimatedDistance,
                ActualDistance = updateTripDto.ActualDistance,
                Notes = updateTripDto.Notes,
                VehicleId = updateTripDto.VehicleId
            };

            var updatedTrip = await _tripService.UpdateTripAsync(trip);
            var responseDto = MapToTripResponseDto(updatedTrip);

            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el viaje con ID {TripId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }



    /// <summary>
    /// Elimina un viaje
    /// </summary>
    /// <param name="id">ID del viaje a eliminar</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTrip(int id)
    {
        try
        {
            var deleted = await _tripService.DeleteTripAsync(id);
            if (!deleted)
            {
                return NotFound($"Viaje con ID {id} no encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el viaje con ID {TripId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene viajes filtrados por estado
    /// </summary>
    /// <param name="status">Estado del viaje</param>
    /// <returns>Lista de viajes con el estado especificado</returns>
    [HttpGet("by-status/{status}")]
    public async Task<ActionResult<IEnumerable<TripResponseDto>>> GetTripsByStatus(TripStatus status)
    {
        try
        {
            var trips = await _tripService.GetTripsByStatusAsync(status);
            var tripDtos = trips.Select(MapToTripResponseDto);

            return Ok(tripDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener viajes por estado {Status}", status);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza solo el estado de un viaje
    /// </summary>
    /// <param name="id">ID del viaje a actualizar</param>
    /// <param name="updateStatusDto">Datos del nuevo estado</param>
    /// <returns>Viaje actualizado</returns>
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<TripResponseDto>> UpdateTripStatus(int id, UpdateTripStatusDto updateStatusDto)
    {
        try
        {
            var updatedTrip = await _tripService.UpdateTripStatusAsync(
                id, 
                updateStatusDto.Status, 
                updateStatusDto.ActualStartDateTime, 
                updateStatusDto.ActualEndDateTime, 
                updateStatusDto.Notes
            );

            var responseDto = MapToTripResponseDto(updatedTrip);
            return Ok(responseDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error al actualizar estado del viaje con ID {TripId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el estado del viaje con ID {TripId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }


}