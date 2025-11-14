using TransportControl.API.DTOs;
using TransportControl.Core.Entities;

namespace TransportControl.API.Mappings;

/// <summary>
/// Extensiones de mapeo para la entidad Operator
/// </summary>
public static class OperatorMappingExtensions
{
    /// <summary>
    /// Convierte una entidad Operator a OperatorResponseDto
    /// </summary>
    /// <param name="operatorEntity">Entidad Operator</param>
    /// <returns>DTO de respuesta</returns>
    public static OperatorResponseDto ToResponseDto(this Operator operatorEntity)
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
    /// Convierte una colección de entidades Operator a OperatorResponseDto
    /// </summary>
    /// <param name="operators">Colección de entidades Operator</param>
    /// <returns>Colección de DTOs de respuesta</returns>
    public static IEnumerable<OperatorResponseDto> ToResponseDto(this IEnumerable<Operator> operators)
    {
        return operators.Select(o => o.ToResponseDto());
    }
}
