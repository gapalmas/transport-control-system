using System.ComponentModel.DataAnnotations;

namespace TransportControl.Core.Entities;

/// <summary>
/// Clase base para todas las entidades del dominio
/// Proporciona campos comunes de identificación y auditoría
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    
    public string? CreatedBy { get; set; }
    
    public string? ModifiedBy { get; set; }
}