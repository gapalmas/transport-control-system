namespace TransportControl.API.DTOs;

public class TripResponseDto
{
    public int Id { get; set; }
    public int OriginId { get; set; }
    public string OriginName { get; set; } = string.Empty;
    public PlaceResponseDto Origin { get; set; } = new();
    public int DestinationId { get; set; }
    public string DestinationName { get; set; } = string.Empty;
    public PlaceResponseDto Destination { get; set; } = new();
    public int OperatorId { get; set; }
    public string OperatorName { get; set; } = string.Empty;
    public OperatorResponseDto Operator { get; set; } = new();
    public DateTime ScheduledStartDateTime { get; set; }
    public DateTime ScheduledEndDateTime { get; set; }
    public DateTime? ActualStartDateTime { get; set; }
    public DateTime? ActualEndDateTime { get; set; }
    public int Status { get; set; }
    public decimal? EstimatedDistance { get; set; }
    public decimal? ActualDistance { get; set; }
    public string? Notes { get; set; }
    public string? VehicleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}

public class OperatorResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? EmployeeId { get; set; }
    public string? LicenseNumber { get; set; }
    public DateTime? LicenseExpiryDate { get; set; }
    public int Status { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? EmergencyPhone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}

public class PlaceResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public int Type { get; set; }
    public int Status { get; set; }
    public bool IsOriginAllowed { get; set; }
    public bool IsDestinationAllowed { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public TimeSpan? OperatingHoursStart { get; set; }
    public TimeSpan? OperatingHoursEnd { get; set; }
    public string? SpecialInstructions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}