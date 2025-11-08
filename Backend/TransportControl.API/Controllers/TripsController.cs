using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportControl.Core.Entities;
using TransportControl.Infrastructure.Data;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de viajes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TripsController : ControllerBase
{
    private readonly TransportDbContext _context;
    private readonly ILogger<TripsController> _logger;

    public TripsController(TransportDbContext context, ILogger<TripsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los viajes con paginación
    /// </summary>
    /// <param name="page">Número de página (por defecto 1)</param>
    /// <param name="pageSize">Tamaño de página (por defecto 10)</param>
    /// <returns>Lista paginada de viajes</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trip>>> GetTrips(int page = 1, int pageSize = 10)
    {
        try
        {
            var trips = await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .OrderByDescending(t => t.ScheduledStartDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(trips);
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
    public async Task<ActionResult<Trip>> GetTrip(int id)
    {
        try
        {
            var trip = await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
            {
                return NotFound($"Viaje con ID {id} no encontrado");
            }

            return Ok(trip);
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
    /// <param name="trip">Datos del viaje a crear</param>
    /// <returns>Viaje creado</returns>
    [HttpPost]
    public async Task<ActionResult<Trip>> CreateTrip(Trip trip)
    {
        try
        {
            // Validaciones básicas
            if (!await _context.Places.AnyAsync(p => p.Id == trip.OriginId))
            {
                return BadRequest("El lugar de origen especificado no existe");
            }

            if (!await _context.Places.AnyAsync(p => p.Id == trip.DestinationId))
            {
                return BadRequest("El lugar de destino especificado no existe");
            }

            if (!await _context.Operators.AnyAsync(o => o.Id == trip.OperatorId))
            {
                return BadRequest("El operador especificado no existe");
            }

            if (trip.ScheduledStartDateTime >= trip.ScheduledEndDateTime)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin");
            }

            // Configurar valores por defecto
            trip.Status = TripStatus.Scheduled;
            trip.CreatedAt = DateTime.UtcNow;
            trip.ModifiedAt = DateTime.UtcNow;

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            // Recargar el viaje con las relaciones
            await _context.Entry(trip)
                .Reference(t => t.Origin)
                .LoadAsync();
            await _context.Entry(trip)
                .Reference(t => t.Destination)
                .LoadAsync();
            await _context.Entry(trip)
                .Reference(t => t.Operator)
                .LoadAsync();

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
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
    /// <param name="trip">Datos actualizados del viaje</param>
    /// <returns>Viaje actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Trip>> UpdateTrip(int id, Trip trip)
    {
        try
        {
            if (id != trip.Id)
            {
                return BadRequest("El ID del viaje no coincide");
            }

            var existingTrip = await _context.Trips.FindAsync(id);
            if (existingTrip == null)
            {
                return NotFound($"Viaje con ID {id} no encontrado");
            }

            // Validaciones
            if (!await _context.Places.AnyAsync(p => p.Id == trip.OriginId))
            {
                return BadRequest("El lugar de origen especificado no existe");
            }

            if (!await _context.Places.AnyAsync(p => p.Id == trip.DestinationId))
            {
                return BadRequest("El lugar de destino especificado no existe");
            }

            if (!await _context.Operators.AnyAsync(o => o.Id == trip.OperatorId))
            {
                return BadRequest("El operador especificado no existe");
            }

            if (trip.ScheduledStartDateTime >= trip.ScheduledEndDateTime)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin");
            }

            // Actualizar campos
            existingTrip.OriginId = trip.OriginId;
            existingTrip.DestinationId = trip.DestinationId;
            existingTrip.OperatorId = trip.OperatorId;
            existingTrip.ScheduledStartDateTime = trip.ScheduledStartDateTime;
            existingTrip.ScheduledEndDateTime = trip.ScheduledEndDateTime;
            existingTrip.Status = trip.Status;
            existingTrip.ActualStartDateTime = trip.ActualStartDateTime;
            existingTrip.ActualEndDateTime = trip.ActualEndDateTime;
            existingTrip.EstimatedDistance = trip.EstimatedDistance;
            existingTrip.ActualDistance = trip.ActualDistance;
            existingTrip.Notes = trip.Notes;
            existingTrip.VehicleId = trip.VehicleId;
            existingTrip.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Recargar con relaciones
            await _context.Entry(existingTrip)
                .Reference(t => t.Origin)
                .LoadAsync();
            await _context.Entry(existingTrip)
                .Reference(t => t.Destination)
                .LoadAsync();
            await _context.Entry(existingTrip)
                .Reference(t => t.Operator)
                .LoadAsync();

            return Ok(existingTrip);
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
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return NotFound($"Viaje con ID {id} no encontrado");
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el viaje con ID {TripId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene viajes por estado
    /// </summary>
    /// <param name="status">Estado del viaje</param>
    /// <returns>Lista de viajes con el estado especificado</returns>
    [HttpGet("by-status/{status}")]
    public async Task<ActionResult<IEnumerable<Trip>>> GetTripsByStatus(TripStatus status)
    {
        try
        {
            var trips = await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.ScheduledStartDateTime)
                .ToListAsync();

            return Ok(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener viajes por estado {Status}", status);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}