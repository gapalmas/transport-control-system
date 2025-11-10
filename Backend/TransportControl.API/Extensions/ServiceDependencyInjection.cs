using TransportControl.Infrastructure.Extensions;
using TransportControl.API.Data;

namespace TransportControl.API.Extensions
{
    /// <summary>
    /// Extensión para configurar la inyección de dependencias y servicios de la aplicación
    /// </summary>
    public static class ServiceDependencyInjection
    {
        /// <summary>
        /// Configura todas las dependencias y servicios de la aplicación
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <returns>Colección de servicios configurada</returns>
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurar servicios básicos
            services.AddControllerServices();
            
            // Configurar Swagger/OpenAPI
            services.AddSwaggerServices();
            
            // Registrar servicios de Infrastructure (incluyendo DbContext)
            services.AddInfrastructure(configuration);
            
            // Configurar CORS
            services.AddCorsServices();
            
            return services;
        }

        /// <summary>
        /// Configura los servicios de los controladores
        /// </summary>
        private static IServiceCollection AddControllerServices(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            return services;
        }

        /// <summary>
        /// Configura Swagger/OpenAPI
        /// </summary>
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "Transport Control API", 
                    Version = "v1",
                    Description = "API para el sistema de control de transporte"
                });
            });

            return services;
        }

        /// <summary>
        /// Configura CORS para permitir comunicación con el frontend
        /// </summary>
        private static IServiceCollection AddCorsServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        /// <summary>
        /// Inicializa los datos semilla de la aplicación
        /// </summary>
        /// <param name="app">Aplicación web</param>
        /// <returns>Aplicación web configurada</returns>
        public static async Task<WebApplication> InitializeSeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error al inicializar los datos de semilla.");
                }
            }

            return app;
        }
    }
}