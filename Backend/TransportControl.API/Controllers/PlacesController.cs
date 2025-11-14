using Microsoft.AspNetCore.Mvc;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.API.DTOs;
using TransportControl.API.Mappings;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de lugares
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Places")]
public class PlacesController : ControllerBase
{
    private readonly IPlaceService _placeService;
    private readonly ILogger<PlacesController> _logger;

    public PlacesController(IPlaceService placeService, ILogger<PlacesController> logger)
    {
        _placeService = placeService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los lugares con paginación
    /// </summary>
    /// <param name="page">Número de página (inicia en 1)</param>
    /// <param name="pageSize">Tamaño de página (máximo 100)</param>
    /// <returns>Lista paginada de lugares</returns>
    [HttpGet]
    public async Task<IActionResult> GetPlaces(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var places = await _placeService.GetPlacesAsync(page, pageSize);
            var placeDtos = places.ToResponseDto().ToList();

            var totalCount = await _placeService.GetPlacesCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Data = placeDtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lugares");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un lugar específico por ID
    /// </summary>
    /// <param name="id">ID del lugar</param>
    /// <returns>Lugar encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PlaceResponseDto>> GetPlace(int id)
    {
        try
        {
            var place = await _placeService.GetPlaceByIdAsync(id);

            if (place == null)
            {
                return NotFound($"Lugar con ID {id} no encontrado");
            }

            var placeDto = place.ToResponseDto();
            return Ok(placeDto);
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
    public async Task<ActionResult<PlaceResponseDto>> CreatePlace(Place place)
    {
        try
        {
            var createdPlace = await _placeService.CreatePlaceAsync(place);
            var placeDto = createdPlace.ToResponseDto();
            
            return CreatedAtAction(nameof(GetPlace), new { id = createdPlace.Id }, placeDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validación falló al crear lugar");
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operación inválida al crear lugar");
            return BadRequest(ex.Message);
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
    public async Task<ActionResult<PlaceResponseDto>> UpdatePlace(int id, Place place)
    {
        try
        {
            if (id != place.Id)
            {
                return BadRequest("El ID del lugar no coincide");
            }

            var updatedPlace = await _placeService.UpdatePlaceAsync(id, place);
            var placeDto = updatedPlace.ToResponseDto();

            return Ok(placeDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validación falló al actualizar lugar con ID {PlaceId}", id);
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Operación inválida al actualizar lugar con ID {PlaceId}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el lugar con ID {PlaceId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene todos los lugares activos para dropdowns (sin paginación)
    /// </summary>
    /// <returns>Lista de lugares activos</returns>
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<PlaceResponseDto>>> GetAllActivePlaces()
    {
        try
        {
            var places = await _placeService.GetActivePlacesAsync();
            var placeDtos = places.ToResponseDto().ToList();

            return Ok(placeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene lugares que pueden ser origen
    /// </summary>
    /// <returns>Lista de lugares origen</returns>
    [HttpGet("origins")]
    public async Task<ActionResult<IEnumerable<PlaceResponseDto>>> GetOriginPlaces()
    {
        try
        {
            var places = await _placeService.GetOriginPlacesAsync();
            var placeDtos = places.ToResponseDto().ToList();

            return Ok(placeDtos);
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
    public async Task<ActionResult<IEnumerable<PlaceResponseDto>>> GetDestinationPlaces()
    {
        try
        {
            var places = await _placeService.GetDestinationPlacesAsync();
            var placeDtos = places.ToResponseDto().ToList();

            return Ok(placeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los lugares destino");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}