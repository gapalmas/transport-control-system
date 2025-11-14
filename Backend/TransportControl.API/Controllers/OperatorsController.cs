using Microsoft.AspNetCore.Mvc;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.API.DTOs;
using TransportControl.API.Mappings;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de operadores
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _operatorService;
    private readonly ILogger<OperatorsController> _logger;

    public OperatorsController(IOperatorService operatorService, ILogger<OperatorsController> logger)
    {
        _operatorService = operatorService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los operadores activos (sin paginación, para dropdowns)
    /// </summary>
    /// <returns>Lista de operadores activos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorResponseDto>>> GetOperators()
    {
        try
        {
            var operators = await _operatorService.GetActiveOperatorsAsync();
            var operatorDtos = operators.ToResponseDto();

            return Ok(operatorDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los operadores");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un operador específico por ID
    /// </summary>
    /// <param name="id">ID del operador</param>
    /// <returns>Operador encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OperatorResponseDto>> GetOperator(int id)
    {
        try
        {
            var operatorEntity = await _operatorService.GetOperatorByIdAsync(id);

            if (operatorEntity == null)
            {
                return NotFound($"Operador con ID {id} no encontrado");
            }

            var operatorDto = operatorEntity.ToResponseDto();
            return Ok(operatorDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el operador con ID {OperatorId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo operador
    /// </summary>
    /// <param name="operatorEntity">Datos del operador a crear</param>
    /// <returns>Operador creado</returns>
    [HttpPost]
    public async Task<ActionResult<Operator>> CreateOperator(Operator operatorEntity)
    {
        try
        {
            var createdOperator = await _operatorService.CreateOperatorAsync(operatorEntity);
            var operatorDto = createdOperator.ToResponseDto();

            return CreatedAtAction(nameof(GetOperator), new { id = createdOperator.Id }, operatorDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear operador");
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de operación al crear operador");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el operador");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un operador existente
    /// </summary>
    /// <param name="id">ID del operador a actualizar</param>
    /// <param name="operatorEntity">Datos actualizados del operador</param>
    /// <returns>Operador actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<OperatorResponseDto>> UpdateOperator(int id, Operator operatorEntity)
    {
        try
        {
            if (id != operatorEntity.Id)
            {
                return BadRequest("El ID del operador no coincide");
            }

            var updatedOperator = await _operatorService.UpdateOperatorAsync(operatorEntity);
            var operatorDto = updatedOperator.ToResponseDto();

            return Ok(operatorDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar operador con ID {OperatorId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el operador con ID {OperatorId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}