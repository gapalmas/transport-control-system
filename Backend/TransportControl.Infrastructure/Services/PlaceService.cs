using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.Infrastructure.Data;

namespace TransportControl.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de lugares
/// </summary>
public class PlaceService : IPlaceService
{
    private readonly TransportDbContext _context;
    private readonly ILogger<PlaceService> _logger;

    public PlaceService(TransportDbContext context, ILogger<PlaceService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los lugares con paginación
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>Lista de lugares</returns>
    public async Task<IEnumerable<Place>> GetPlacesAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Obteniendo lugares - Página: {Page}, Tamaño: {PageSize}", page, pageSize);
            
            // Validar parámetros de paginación
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            return await _context.Places
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugares con paginación");
            throw;
        }
    }

    /// <summary>
    /// Obtiene la cantidad total de lugares
    /// </summary>
    /// <returns>Cantidad total de lugares</returns>
    public async Task<int> GetPlacesCountAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo conteo total de lugares");
            return await _context.Places.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener conteo de lugares");
            throw;
        }
    }

    /// <summary>
    /// Obtiene un lugar por su ID
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>Lugar encontrado o null</returns>
    public async Task<Place?> GetPlaceByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo lugar con ID: {PlaceId}", id);
            return await _context.Places.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugar con ID: {PlaceId}", id);
            throw;
        }
    }

    /// <summary>
    /// Crea un nuevo lugar
    /// </summary>
    /// <param name="place">Lugar a crear</param>
    /// <returns>Lugar creado</returns>
    public async Task<Place> CreatePlaceAsync(Place place)
    {
        try
        {
            _logger.LogInformation("Creando nuevo lugar: {PlaceName}", place.Name);
            
            // Validaciones
            if (string.IsNullOrWhiteSpace(place.Name))
            {
                throw new ArgumentException("El nombre del lugar es requerido");
            }

            // Verificar duplicado por código si se proporciona
            if (!string.IsNullOrEmpty(place.Code))
            {
                var existingByCode = await ExistsByCodeAsync(place.Code);
                if (existingByCode)
                {
                    throw new InvalidOperationException("Ya existe un lugar con este código");
                }
            }

            place.CreatedAt = DateTime.UtcNow;
            place.ModifiedAt = DateTime.UtcNow;

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Lugar creado exitosamente con ID: {PlaceId}", place.Id);
            return place;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear lugar: {PlaceName}", place.Name);
            throw;
        }
    }

    /// <summary>
    /// Actualiza un lugar existente
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <param name="place">Lugar con los datos actualizados</param>
    /// <returns>Lugar actualizado</returns>
    public async Task<Place> UpdatePlaceAsync(int id, Place place)
    {
        try
        {
            _logger.LogInformation("Actualizando lugar con ID: {PlaceId}", id);
            
            var existing = await _context.Places.FindAsync(id);
            if (existing == null)
            {
                throw new InvalidOperationException($"Lugar con ID {id} no encontrado");
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(place.Name))
            {
                throw new ArgumentException("El nombre del lugar es requerido");
            }

            // Verificar duplicado por código si se proporciona
            if (!string.IsNullOrEmpty(place.Code))
            {
                var existingByCode = await ExistsByCodeAsync(place.Code, id);
                if (existingByCode)
                {
                    throw new InvalidOperationException("Ya existe otro lugar con este código");
                }
            }

            // Actualizar campos
            existing.Name = place.Name;
            existing.Code = place.Code;
            existing.Description = place.Description;
            existing.Address = place.Address;
            existing.City = place.City;
            existing.State = place.State;
            existing.Country = place.Country;
            existing.PostalCode = place.PostalCode;
            existing.Latitude = place.Latitude;
            existing.Longitude = place.Longitude;
            existing.Type = place.Type;
            existing.Status = place.Status;
            existing.IsOriginAllowed = place.IsOriginAllowed;
            existing.IsDestinationAllowed = place.IsDestinationAllowed;
            existing.ContactPerson = place.ContactPerson;
            existing.ContactPhone = place.ContactPhone;
            existing.ContactEmail = place.ContactEmail;
            existing.OperatingHoursStart = place.OperatingHoursStart;
            existing.OperatingHoursEnd = place.OperatingHoursEnd;
            existing.SpecialInstructions = place.SpecialInstructions;
            existing.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Lugar actualizado exitosamente: {PlaceId}", id);
            return existing;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar lugar con ID: {PlaceId}", id);
            throw;
        }
    }

    /// <summary>
    /// Elimina un lugar
    /// </summary>
    /// <param name="id">ID del lugar a eliminar</param>
    /// <returns>True si se eliminó correctamente</returns>
    public async Task<bool> DeletePlaceAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando lugar con ID: {PlaceId}", id);
            
            var place = await _context.Places.FindAsync(id);
            if (place == null)
            {
                return false;
            }

            // Verificar si tiene viajes como origen o destino
            var hasTripsAsOrigin = await _context.Trips.AnyAsync(t => t.OriginId == id);
            var hasTripsAsDestination = await _context.Trips.AnyAsync(t => t.DestinationId == id);

            if (hasTripsAsOrigin || hasTripsAsDestination)
            {
                throw new InvalidOperationException("No se puede eliminar el lugar porque está siendo usado en viajes");
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Lugar eliminado exitosamente: {PlaceId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar lugar con ID: {PlaceId}", id);
            throw;
        }
    }

    /// <summary>
    /// Obtiene lugares que pueden ser origen
    /// </summary>
    /// <returns>Lista de lugares origen</returns>
    public async Task<IEnumerable<Place>> GetOriginPlacesAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo lugares origen");
            return await _context.Places
                .Where(p => p.Status == PlaceStatus.Active && p.IsOriginAllowed)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugares origen");
            throw;
        }
    }

    /// <summary>
    /// Obtiene lugares que pueden ser destino
    /// </summary>
    /// <returns>Lista de lugares destino</returns>
    public async Task<IEnumerable<Place>> GetDestinationPlacesAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo lugares destino");
            return await _context.Places
                .Where(p => p.Status == PlaceStatus.Active && p.IsDestinationAllowed)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugares destino");
            throw;
        }
    }

    /// <summary>
    /// Obtiene todos los lugares activos
    /// </summary>
    /// <returns>Lista de lugares activos</returns>
    public async Task<IEnumerable<Place>> GetActivePlacesAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo lugares activos");
            return await _context.Places
                .Where(p => p.Status == PlaceStatus.Active)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugares activos");
            throw;
        }
    }

    /// <summary>
    /// Verifica si existe un lugar con el código especificado
    /// </summary>
    /// <param name="code">Código del lugar</param>
    /// <param name="excludeId">ID del lugar a excluir de la búsqueda (para actualizaciones)</param>
    /// <returns>True si existe un lugar con ese código</returns>
    public async Task<bool> ExistsByCodeAsync(string code, int? excludeId = null)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de lugar con código: {Code}", code);
            
            var query = _context.Places.Where(p => p.Code == code);
            
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de lugar con código: {Code}", code);
            throw;
        }
    }
}