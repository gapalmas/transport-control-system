namespace TransportControl.Core.Entities;

/// <summary>
/// Catálogo de operadores que pueden realizar viajes
/// </summary>
public class Operator : BaseEntity
{
    
    // Información personal
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    
    // Información de contacto
    public string? Email { get; set; }
    public string? Phone { get; set; }
    
    // Información laboral
    public string? EmployeeId { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public OperatorStatus Status { get; set; } = OperatorStatus.Active;
    
    // Información adicional
    public DateTime? DateOfBirth { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    
    // Relaciones
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}

/// <summary>
/// Estados posibles de un operador
/// </summary>
public enum OperatorStatus
{
    Active = 1,         // Activo
    Inactive = 2,       // Inactivo
    Suspended = 3,      // Suspendido
    OnLeave = 4         // De licencia
}