using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.Infrastructure.Data;

namespace TransportControl.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de viajes
/// </summary>
public class TripService : ITripService
{
    private readonly TransportDbContext _context;
    private readonly ILogger<TripService> _logger;

    public TripService(TransportDbContext context, ILogger<TripService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Trip>> GetTripsAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            return await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .OrderByDescending(t => t.ScheduledStartDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener viajes con paginación. Página: {Page}, Tamaño: {PageSize}", page, pageSize);
            throw;
        }
    }

    public async Task<Trip?> GetTripByIdAsync(int id)
    {
        try
        {
            return await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener viaje por ID: {TripId}", id);
            throw;
        }
    }

    public async Task<Trip> CreateTripAsync(Trip trip)
    {
        try
        {
            // Validar el viaje
            var validationErrors = await ValidateTripAsync(trip);
            if (validationErrors.Any())
            {
                throw new ArgumentException($"Errores de validación: {string.Join(", ", validationErrors)}");
            }

            trip.CreatedAt = DateTime.UtcNow;
            trip.ModifiedAt = DateTime.UtcNow;

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            // Recargar con relaciones
            return await GetTripByIdAsync(trip.Id) ?? trip;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear viaje");
            throw;
        }
    }

    public async Task<Trip> UpdateTripAsync(Trip trip)
    {
        try
        {
            var existingTrip = await _context.Trips.FindAsync(trip.Id);
            if (existingTrip == null)
            {
                throw new ArgumentException($"Viaje con ID {trip.Id} no encontrado");
            }

            // Validar el viaje
            var validationErrors = await ValidateTripAsync(trip);
            if (validationErrors.Any())
            {
                throw new ArgumentException($"Errores de validación: {string.Join(", ", validationErrors)}");
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
            return await GetTripByIdAsync(existingTrip.Id) ?? existingTrip;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar viaje con ID: {TripId}", trip.Id);
            throw;
        }
    }

    public async Task<Trip> UpdateTripStatusAsync(int tripId, TripStatus status, DateTime? actualStartDateTime = null, DateTime? actualEndDateTime = null, string? notes = null)
    {
        try
        {
            var existingTrip = await GetTripByIdAsync(tripId);
            if (existingTrip == null)
            {
                throw new ArgumentException($"Viaje con ID {tripId} no encontrado");
            }

            // Actualizar el estado
            existingTrip.Status = status;
            existingTrip.ModifiedAt = DateTime.UtcNow;

            // Actualizar fechas según el estado
            switch (status)
            {
                case TripStatus.InProgress:
                    existingTrip.ActualStartDateTime = actualStartDateTime ?? DateTime.UtcNow;
                    break;
                case TripStatus.Completed:
                    existingTrip.ActualEndDateTime = actualEndDateTime ?? DateTime.UtcNow;
                    break;
            }

            // Actualizar notas si se proporcionan
            if (!string.IsNullOrEmpty(notes))
            {
                existingTrip.Notes = string.IsNullOrEmpty(existingTrip.Notes) ? notes : $"{existingTrip.Notes}; {notes}";
            }

            await _context.SaveChangesAsync();

            return existingTrip;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado del viaje con ID: {TripId}", tripId);
            throw;
        }
    }

    public async Task<bool> DeleteTripAsync(int id)
    {
        try
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
            {
                return false;
            }

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar viaje con ID: {TripId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Trip>> GetTripsByStatusAsync(TripStatus status)
    {
        try
        {
            return await _context.Trips
                .Include(t => t.Origin)
                .Include(t => t.Destination)
                .Include(t => t.Operator)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.ScheduledStartDateTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener viajes por estado: {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<string>> ValidateTripAsync(Trip trip)
    {
        var errors = new List<string>();

        try
        {
            // Validar origen
            if (!await _context.Places.AnyAsync(p => p.Id == trip.OriginId))
            {
                errors.Add("El lugar de origen especificado no existe");
            }

            // Validar destino
            if (!await _context.Places.AnyAsync(p => p.Id == trip.DestinationId))
            {
                errors.Add("El lugar de destino especificado no existe");
            }

            // Validar que origen y destino no sean iguales
            if (trip.OriginId == trip.DestinationId)
            {
                errors.Add("El origen y destino no pueden ser iguales");
            }

            // Validar operador
            if (!await _context.Operators.AnyAsync(o => o.Id == trip.OperatorId))
            {
                errors.Add("El operador especificado no existe");
            }

            // Validar fechas
            if (trip.ScheduledStartDateTime >= trip.ScheduledEndDateTime)
            {
                errors.Add("La fecha de inicio debe ser anterior a la fecha de fin");
            }

            // Validar que la fecha programada no sea en el pasado (solo para nuevos viajes)
            if (trip.Id == 0 && trip.ScheduledStartDateTime < DateTime.Now.AddHours(-1))
            {
                errors.Add("La fecha programada no puede ser en el pasado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar viaje");
            errors.Add("Error interno durante la validación");
        }

        return errors;
    }
}