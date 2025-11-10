using TransportControl.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure all dependencies & services
builder.Services.AddDependencyInjection(builder.Configuration);

var app = builder.Build();

// Init seeder
await app.InitializeSeedDataAsync();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport Control API v1");
        c.RoutePrefix = string.Empty; // Para que Swagger sea la página inicial
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
