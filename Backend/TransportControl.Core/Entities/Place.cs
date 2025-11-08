namespace TransportControl.Core.Entities;

/// <summary>
/// Catálogo de lugares que pueden ser origen o destino de viajes
/// </summary>
public class Place : BaseEntity
{
    
    // Información básica
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    
    // Ubicación
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; } = "Mexico";
    public string? PostalCode { get; set; }
    
    // Coordenadas geográficas
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    // Tipo de lugar
    public PlaceType Type { get; set; } = PlaceType.Terminal;
    
    // Estado y configuración
    public PlaceStatus Status { get; set; } = PlaceStatus.Active;
    public bool IsOriginAllowed { get; set; } = true;
    public bool IsDestinationAllowed { get; set; } = true;
    
    // Información adicional
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public TimeSpan? OperatingHoursStart { get; set; }
    public TimeSpan? OperatingHoursEnd { get; set; }
    public string? SpecialInstructions { get; set; }
    
    // Relaciones
    public ICollection<Trip> TripsAsOrigin { get; set; } = new List<Trip>();
    public ICollection<Trip> TripsAsDestination { get; set; } = new List<Trip>();
}

/// <summary>
/// Tipos de lugar
/// </summary>
public enum PlaceType
{
    Terminal = 1,       // Terminal de transporte
    Station = 2,        // Estación
    Warehouse = 3,      // Almacén
    Port = 4,          // Puerto
    Airport = 5,       // Aeropuerto
    DistributionCenter = 6, // Centro de distribución
    CustomerSite = 7,   // Sitio del cliente
    Other = 8          // Otro
}

/// <summary>
/// Estados posibles de un lugar
/// </summary>
public enum PlaceStatus
{
    Active = 1,         // Activo
    Inactive = 2,       // Inactivo
    Maintenance = 3,    // En mantenimiento
    Closed = 4         // Cerrado
}