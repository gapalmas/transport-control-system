using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransportControl.Core.Entities;
using TransportControl.Core.Interfaces;
using TransportControl.Infrastructure.Data;

namespace TransportControl.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de operadores
/// </summary>
public class OperatorService : IOperatorService
{
    private readonly TransportDbContext _context;
    private readonly ILogger<OperatorService> _logger;

    public OperatorService(TransportDbContext context, ILogger<OperatorService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Operator>> GetOperatorsAsync()
    {
        try
        {
            return await _context.Operators
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener operadores");
            throw;
        }
    }

    public async Task<Operator?> GetOperatorByIdAsync(int id)
    {
        try
        {
            return await _context.Operators.FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener operador por ID: {OperatorId}", id);
            throw;
        }
    }

    public async Task<Operator> CreateOperatorAsync(Operator operatorEntity)
    {
        try
        {
            operatorEntity.CreatedAt = DateTime.UtcNow;
            operatorEntity.ModifiedAt = DateTime.UtcNow;

            _context.Operators.Add(operatorEntity);
            await _context.SaveChangesAsync();

            return operatorEntity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear operador");
            throw;
        }
    }

    public async Task<Operator> UpdateOperatorAsync(Operator operatorEntity)
    {
        try
        {
            var existingOperator = await _context.Operators.FindAsync(operatorEntity.Id);
            if (existingOperator == null)
            {
                throw new ArgumentException($"Operador con ID {operatorEntity.Id} no encontrado");
            }

            // Actualizar campos
            existingOperator.FirstName = operatorEntity.FirstName;
            existingOperator.LastName = operatorEntity.LastName;
            existingOperator.Email = operatorEntity.Email;
            existingOperator.Phone = operatorEntity.Phone;
            existingOperator.EmployeeId = operatorEntity.EmployeeId;
            existingOperator.LicenseNumber = operatorEntity.LicenseNumber;
            existingOperator.LicenseExpiryDate = operatorEntity.LicenseExpiryDate;
            existingOperator.Status = operatorEntity.Status;
            existingOperator.DateOfBirth = operatorEntity.DateOfBirth;
            existingOperator.HireDate = operatorEntity.HireDate;
            existingOperator.Address = operatorEntity.Address;
            existingOperator.EmergencyContact = operatorEntity.EmergencyContact;
            existingOperator.EmergencyPhone = operatorEntity.EmergencyPhone;
            existingOperator.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingOperator;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar operador con ID: {OperatorId}", operatorEntity.Id);
            throw;
        }
    }

    public async Task<bool> DeleteOperatorAsync(int id)
    {
        try
        {
            var operatorEntity = await _context.Operators.FindAsync(id);
            if (operatorEntity == null)
            {
                return false;
            }

            // Verificar si tiene viajes asignados
            var hasTrips = await _context.Trips.AnyAsync(t => t.OperatorId == id);
            if (hasTrips)
            {
                throw new InvalidOperationException("No se puede eliminar el operador porque tiene viajes asignados");
            }

            _context.Operators.Remove(operatorEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar operador con ID: {OperatorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Operator>> GetActiveOperatorsAsync()
    {
        try
        {
            return await _context.Operators
                .Where(o => o.Status == OperatorStatus.Active)
                .OrderBy(o => o.FirstName)
                .ThenBy(o => o.LastName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener operadores activos");
            throw;
        }
    }
}