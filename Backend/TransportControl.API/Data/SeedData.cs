using TransportControl.Core.Entities;
using TransportControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TransportControl.API.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = serviceProvider.GetRequiredService<TransportDbContext>();
        
        // Aplicar migraciones pendientes
        await context.Database.MigrateAsync();
        
        // Verificar si ya hay datos
        if (await context.Places.AnyAsync() || await context.Operators.AnyAsync())
        {
            return; // Ya hay datos
        }

        // Crear lugares de prueba
        var places = new List<Place>
        {
            new()
            {
                Name = "Ciudad de México",
                Address = "Centro Histórico, Ciudad de México",
                Latitude = 19.4326m,
                Longitude = -99.1332m,
                Status = PlaceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Guadalajara",
                Address = "Centro de Guadalajara, Jalisco",
                Latitude = 20.6597m,
                Longitude = -103.3496m,
                Status = PlaceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Monterrey",
                Address = "Centro de Monterrey, Nuevo León",
                Latitude = 25.6866m,
                Longitude = -100.3161m,
                Status = PlaceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Puebla",
                Address = "Centro Histórico, Puebla",
                Latitude = 19.0414m,
                Longitude = -98.2063m,
                Status = PlaceStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Cancún",
                Address = "Zona Hotelera, Cancún, Quintana Roo",
                Latitude = 21.1619m,
                Longitude = -86.8515m,
                Status = PlaceStatus.Active,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Places.AddRangeAsync(places);
        await context.SaveChangesAsync();

        // Crear operadores de prueba
        var operators = new List<Operator>
        {
            new()
            {
                FirstName = "Juan",
                LastName = "Pérez González",
                Email = "juan.perez@transport.com",
                Phone = "+52 55 1234 5678",
                EmployeeId = "EMP001",
                LicenseNumber = "LIC12345678",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(2),
                Status = OperatorStatus.Active,
                DateOfBirth = new DateTime(1985, 5, 15),
                HireDate = new DateTime(2020, 1, 15),
                Address = "Calle Principal 123, Ciudad de México",
                EmergencyContact = "María Pérez",
                EmergencyPhone = "+52 55 8765 4321",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                FirstName = "Ana",
                LastName = "García López",
                Email = "ana.garcia@transport.com",
                Phone = "+52 33 2345 6789",
                EmployeeId = "EMP002",
                LicenseNumber = "LIC87654321",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(3),
                Status = OperatorStatus.Active,
                DateOfBirth = new DateTime(1990, 8, 22),
                HireDate = new DateTime(2021, 3, 10),
                Address = "Avenida Reforma 456, Guadalajara",
                EmergencyContact = "Carlos García",
                EmergencyPhone = "+52 33 9876 5432",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                FirstName = "Luis",
                LastName = "Martínez Rodríguez",
                Email = "luis.martinez@transport.com",
                Phone = "+52 81 3456 7890",
                EmployeeId = "EMP003",
                LicenseNumber = "LIC11223344",
                LicenseExpiryDate = DateTime.UtcNow.AddYears(1),
                Status = OperatorStatus.Active,
                DateOfBirth = new DateTime(1982, 12, 3),
                HireDate = new DateTime(2019, 7, 20),
                Address = "Boulevard Norte 789, Monterrey",
                EmergencyContact = "Elena Martínez",
                EmergencyPhone = "+52 81 1122 3344",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                FirstName = "Carmen",
                LastName = "Hernández Silva",
                Email = "carmen.hernandez@transport.com",
                Phone = "+52 22 4567 8901",
                EmployeeId = "EMP004",
                LicenseNumber = "LIC55667788",
                LicenseExpiryDate = DateTime.UtcNow.AddMonths(6),
                Status = OperatorStatus.Active,
                DateOfBirth = new DateTime(1988, 4, 18),
                HireDate = new DateTime(2022, 1, 5),
                Address = "Calle 16 de Septiembre 321, Puebla",
                EmergencyContact = "Roberto Hernández",
                EmergencyPhone = "+52 22 5566 7788",
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Operators.AddRangeAsync(operators);
        await context.SaveChangesAsync();

        Console.WriteLine("Datos de semilla agregados exitosamente.");
    }
}