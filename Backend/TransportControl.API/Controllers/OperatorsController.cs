using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportControl.Core.Entities;
using TransportControl.Infrastructure.Data;

namespace TransportControl.API.Controllers;

/// <summary>
/// Controlador para la gestión de operadores
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OperatorsController : ControllerBase
{
    private readonly TransportDbContext _context;
    private readonly ILogger<OperatorsController> _logger;

    public OperatorsController(TransportDbContext context, ILogger<OperatorsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los operadores
    /// </summary>
    /// <returns>Lista de operadores</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Operator>>> GetOperators()
    {
        try
        {
            var operators = await _context.Operators
                .Where(o => o.Status == OperatorStatus.Active)
                .OrderBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .ToListAsync();

            return Ok(operators);
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
    public async Task<ActionResult<Operator>> GetOperator(int id)
    {
        try
        {
            var operatorEntity = await _context.Operators.FindAsync(id);

            if (operatorEntity == null)
            {
                return NotFound($"Operador con ID {id} no encontrado");
            }

            return Ok(operatorEntity);
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
            // Validaciones
            if (string.IsNullOrWhiteSpace(operatorEntity.FirstName))
            {
                return BadRequest("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(operatorEntity.LastName))
            {
                return BadRequest("El apellido es requerido");
            }

            // Verificar duplicados
            if (!string.IsNullOrEmpty(operatorEntity.EmployeeId))
            {
                var existingByEmployeeId = await _context.Operators
                    .AnyAsync(o => o.EmployeeId == operatorEntity.EmployeeId);
                if (existingByEmployeeId)
                {
                    return BadRequest("Ya existe un operador con este ID de empleado");
                }
            }

            if (!string.IsNullOrEmpty(operatorEntity.Email))
            {
                var existingByEmail = await _context.Operators
                    .AnyAsync(o => o.Email == operatorEntity.Email);
                if (existingByEmail)
                {
                    return BadRequest("Ya existe un operador con este email");
                }
            }

            operatorEntity.CreatedAt = DateTime.UtcNow;
            operatorEntity.ModifiedAt = DateTime.UtcNow;

            _context.Operators.Add(operatorEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOperator), new { id = operatorEntity.Id }, operatorEntity);
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
    public async Task<ActionResult<Operator>> UpdateOperator(int id, Operator operatorEntity)
    {
        try
        {
            if (id != operatorEntity.Id)
            {
                return BadRequest("El ID del operador no coincide");
            }

            var existing = await _context.Operators.FindAsync(id);
            if (existing == null)
            {
                return NotFound($"Operador con ID {id} no encontrado");
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(operatorEntity.FirstName))
            {
                return BadRequest("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(operatorEntity.LastName))
            {
                return BadRequest("El apellido es requerido");
            }

            // Actualizar campos
            existing.FirstName = operatorEntity.FirstName;
            existing.LastName = operatorEntity.LastName;
            existing.Email = operatorEntity.Email;
            existing.Phone = operatorEntity.Phone;
            existing.EmployeeId = operatorEntity.EmployeeId;
            existing.LicenseNumber = operatorEntity.LicenseNumber;
            existing.LicenseExpiryDate = operatorEntity.LicenseExpiryDate;
            existing.Status = operatorEntity.Status;
            existing.DateOfBirth = operatorEntity.DateOfBirth;
            existing.HireDate = operatorEntity.HireDate;
            existing.Address = operatorEntity.Address;
            existing.EmergencyContact = operatorEntity.EmergencyContact;
            existing.EmergencyPhone = operatorEntity.EmergencyPhone;
            existing.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el operador con ID {OperatorId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}