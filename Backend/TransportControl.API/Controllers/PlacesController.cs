using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportControl.Core.Entities;
using TransportControl.Infrastructure.Data;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de lugares
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlacesController : ControllerBase
{
    private readonly TransportDbContext _context;
    private readonly ILogger<PlacesController> _logger;

    public PlacesController(TransportDbContext context, ILogger<PlacesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los lugares
    /// </summary>
    /// <returns>Lista de lugares</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
    {
        try
        {
            var places = await _context.Places
                .Where(p => p.Status == PlaceStatus.Active)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(places);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un lugar específico por ID
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>Lugar encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Place>> GetPlace(int id)
    {
        try
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
            {
                return NotFound($"Lugar con ID {id} no encontrado");
            }

            return Ok(place);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el lugar con ID {PlaceId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo lugar
    /// </summary>
    /// <param name="place">Datos del lugar a crear</param>
    /// <returns>Lugar creado</returns>
    [HttpPost]
    public async Task<ActionResult<Place>> CreatePlace(Place place)
    {
        try
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(place.Name))
            {
                return BadRequest("El nombre del lugar es requerido");
            }

            // Verificar duplicado por código si se proporciona
            if (!string.IsNullOrEmpty(place.Code))
            {
                var existingByCode = await _context.Places
                    .AnyAsync(p => p.Code == place.Code);
                if (existingByCode)
                {
                    return BadRequest("Ya existe un lugar con este código");
                }
            }

            place.CreatedAt = DateTime.UtcNow;
            place.ModifiedAt = DateTime.UtcNow;

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlace), new { id = place.Id }, place);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el lugar");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un lugar existente
    /// </summary>
    /// <param name="id">ID del lugar a actualizar</param>
    /// <param name="place">Datos actualizados del lugar</param>
    /// <returns>Lugar actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Place>> UpdatePlace(int id, Place place)
    {
        try
        {
            if (id != place.Id)
            {
                return BadRequest("El ID del lugar no coincide");
            }

            var existing = await _context.Places.FindAsync(id);
            if (existing == null)
            {
                return NotFound($"Lugar con ID {id} no encontrado");
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(place.Name))
            {
                return BadRequest("El nombre del lugar es requerido");
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

            return Ok(existing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el lugar con ID {PlaceId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene lugares que pueden ser origen
    /// </summary>
    /// <returns>Lista de lugares origen</returns>
    [HttpGet("origins")]
    public async Task<ActionResult<IEnumerable<Place>>> GetOriginPlaces()
    {
        try
        {
            var places = await _context.Places
                .Where(p => p.Status == PlaceStatus.Active && p.IsOriginAllowed)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(places);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares origen");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene lugares que pueden ser destino
    /// </summary>
    /// <returns>Lista de lugares destino</returns>
    [HttpGet("destinations")]
    public async Task<ActionResult<IEnumerable<Place>>> GetDestinationPlaces()
    {
        try
        {
            var places = await _context.Places
                .Where(p => p.Status == PlaceStatus.Active && p.IsDestinationAllowed)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(places);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares destino");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}