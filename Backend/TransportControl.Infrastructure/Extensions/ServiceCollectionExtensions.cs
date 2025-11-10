using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransportControl.Core.Interfaces;
using TransportControl.Infrastructure.Data;

namespace TransportControl.Infrastructure.Extensions;

/// <summary>
/// Extensiones para registrar los servicios de Infrastructure
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra los servicios de infraestructura en el contenedor de dependencias
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios modificada</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registrar DbContext
        services.AddDbContext<TransportDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TransportDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });

            // Configuraciones adicionales para desarrollo
            var enableSensitiveLogging = configuration["EnableSensitiveDataLogging"];
            if (bool.TryParse(enableSensitiveLogging, out var sensitiveLogging) && sensitiveLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            var enableDetailedErrors = configuration["EnableDetailedErrors"];
            if (bool.TryParse(enableDetailedErrors, out var detailedErrors) && detailedErrors)
            {
                options.EnableDetailedErrors();
            }
        });

        // Registrar servicios de dominio
        services.AddScoped<ITripService, Services.TripService>();
        services.AddScoped<IOperatorService, Services.OperatorService>();
        services.AddScoped<IPlaceService, Services.PlaceService>();

        return services;
    }
}