# Sistema de Control de Transporte

## Descripción
Aplicación web para el registro y control de viajes de transporte, desarrollada con Clean Architecture.

## Tecnologías
- **Backend**: .NET 8 con Clean Architecture
- **Frontend**: Angular
- **Base de datos**: SQL Server
- **Contenedores**: Docker
- **Despliegue**: Docker Hub, Vercel

# Sistema de Control de Transporte

## Descripción
Sistema web para el registro y control de viajes de transporte desarrollado con .NET 8 y Clean Architecture. Permite gestionar viajes programados con información completa de origen, destino, operadores y seguimiento de estados.

## Estado del Proyecto
✅ **Backend API REST completamente funcional**
- Arquitectura Clean implementada
- Base de datos creada y migrada
- Endpoints CRUD operativos
- Swagger UI configurado

🔄 **Frontend Angular** (Pendiente)

## Tecnologías Implementadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 8.0** - ORM con Code First
- **SQL Server** - Base de datos principal
- **Swagger/OpenAPI** - Documentación automática de API
- **Clean Architecture** - Separación de responsabilidades

### Base de Datos
- **Proveedor**: SQL Server (Somee.com)
- **Estrategia**: Code First con migraciones
- **Características**: Campos de auditoría, índices optimizados, relaciones con integridad referencial

## Arquitectura del Backend

```
Source/Backend/
├── TransportControl.Core/           # Dominio
│   └── Entities/                    # Entidades del negocio
│       ├── BaseEntity.cs           # Clase base con auditoría
│       ├── Trip.cs                 # Viaje principal
│       ├── Operator.cs             # Operadores de transporte
│       └── Place.cs                # Lugares (origen/destino)
├── TransportControl.Infrastructure/ # Infraestructura
│   ├── Data/
│   │   └── TransportDbContext.cs   # Contexto de EF Core
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs # Registro de servicios
│   └── Migrations/                 # Migraciones de base de datos
└── TransportControl.API/           # Presentación
    ├── Controllers/                # Controladores REST
    │   ├── TripsController.cs      # CRUD de viajes
    │   ├── OperatorsController.cs  # Gestión de operadores
    │   └── PlacesController.cs     # Gestión de lugares
    ├── Program.cs                  # Configuración de la app
    └── appsettings.json           # Configuración y conexión DB
```

## Modelo de Datos

### Entidades Principales

**Trip (Viajes)**
- Información de origen y destino
- Fechas programadas y reales
- Estado del viaje (Programado, En progreso, Completado, etc.)
- Operador asignado
- Distancias estimadas y reales
- Notas y ID de vehículo

**Operator (Operadores)**
- Información personal y de contacto
- Datos laborales (ID empleado, licencia)
- Estado del operador
- Información de emergencia

**Place (Lugares)**
- Información básica y ubicación
- Coordenadas geográficas
- Tipo de lugar (Terminal, Estación, Puerto, etc.)
- Configuración de permisos (origen/destino)
- Horarios de operación

### Características de las Entidades
- **BaseEntity**: Campos de auditoría automáticos (Id, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
- **Índices optimizados**: Para consultas frecuentes y restricciones únicas
- **Relaciones configuradas**: Foreign keys con restricciones de integridad

## API Endpoints

### Viajes
- `GET /api/trips` - Listar viajes (con paginación)
- `GET /api/trips/{id}` - Obtener viaje específico
- `POST /api/trips` - Crear nuevo viaje
- `PUT /api/trips/{id}` - Actualizar viaje
- `DELETE /api/trips/{id}` - Eliminar viaje
- `GET /api/trips/by-status/{status}` - Filtrar por estado

### Operadores
- `GET /api/operators` - Listar operadores activos
- `GET /api/operators/{id}` - Obtener operador específico
- `POST /api/operators` - Crear nuevo operador
- `PUT /api/operators/{id}` - Actualizar operador

### Lugares
- `GET /api/places` - Listar lugares activos
- `GET /api/places/{id}` - Obtener lugar específico
- `POST /api/places` - Crear nuevo lugar
- `PUT /api/places/{id}` - Actualizar lugar
- `GET /api/places/origins` - Lugares habilitados como origen
- `GET /api/places/destinations` - Lugares habilitados como destino

## Instalación y Ejecución

### Prerrequisitos
- .NET 8 SDK
- Acceso a SQL Server
- Git

### Configuración

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd transport-proyect/Source/Backend
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Configurar base de datos**
   - Actualizar cadena de conexión en `appsettings.json`
   - La migración ya está aplicada a la base de datos

4. **Ejecutar la aplicación**
```bash
dotnet run --project TransportControl.API
```

5. **Acceder a Swagger UI**
   - URL: `http://localhost:5153`
   - Documentación interactiva de todos los endpoints

### Comandos de Entity Framework

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project TransportControl.Infrastructure --startup-project TransportControl.API

# Aplicar migraciones
dotnet ef database update --project TransportControl.Infrastructure --startup-project TransportControl.API

# Revertir migración
dotnet ef migrations remove --project TransportControl.Infrastructure --startup-project TransportControl.API
```

## Configuración de Desarrollo

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "workstation id=transportapp.mssql.somee.com;packet size=4096;user id=gapalmas_SQLLogin_1;pwd=oh4tl59sdk;data source=transportapp.mssql.somee.com;persist security info=False;initial catalog=transportapp;TrustServerCertificate=True"
  },
  "EnableSensitiveDataLogging": false,
  "EnableDetailedErrors": true
}
```

## Sugerencias Próximos Pasos
1. **Frontend Angular**: Crear aplicación cliente
2. **Autenticación**: Implementar JWT
3. **Validaciones**: Agregar Data Annotations más robustas
4. **Logging**: Configurar Serilog
5. **Tests**: Unit tests y integration tests
6. **Docker**: Containerización completa
7. **CI/CD**: Pipeline de despliegue

## Contribución
1. Fork el proyecto
2. Crea una rama: `git checkout -b feature/nueva-caracteristica`
3. Commit cambios: `git commit -m 'Agregar nueva característica'`
4. Push: `git push origin feature/nueva-caracteristica`
5. Crear Pull Request

## Características Principales

### Funcionalidades
- ✅ Registro de viajes
- ✅ Modificación de viajes
- ✅ Gestión de catálogos (Operadores, Lugares)
- ✅ Consulta de viajes

### Información del Viaje
- **Origen**: Lugar de inicio del viaje
- **Destino**: Lugar donde termina el viaje
- **Fecha y Hora de Inicio Programado**
- **Fecha y Hora de Fin Programado**
- **Operador**: Persona que realiza el viaje

## Instalación y Ejecución

### Backend (.NET 8)
```bash
cd Source/Backend
dotnet restore
dotnet build
dotnet run --project WebAPI
```

### Frontend (Angular)
```bash
cd Source/Frontend
npm install
ng serve
```

### Docker
```bash
docker-compose up -d
```

## Base de Datos

El proyecto utiliza Code First con Entity Framework Core para la gestión de la base de datos.

### Migraciones
```bash
cd Source/Backend/Infrastructure
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.