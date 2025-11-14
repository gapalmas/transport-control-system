# Sistema de Control de Transporte

## Descripción
Sistema web para el registro y control de viajes de transporte desarrollado con .NET 8 y Clean Architecture. Permite gestionar viajes programados con información completa de origen, destino, operadores y seguimiento de estados.

## 🚀 Inicio Rápido con Docker

**La forma más rápida de ejecutar el proyecto completo:**

```bash
cd Source
docker compose up
```

**URLs de acceso:**
- Frontend: http://localhost (ej: http://localhost/trips)
- Backend API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger

## Estado del Proyecto
✅ **Backend API REST completamente funcional**
- Arquitectura Clean implementada
- Base de datos creada y migrada
- Endpoints CRUD operativos
- Swagger UI configurado

✅ **Frontend Angular completamente funcional**
- Interfaz completa con Angular Material
- CRUD de Viajes, Operadores y Lugares
- Navegación y routing configurado

✅ **Despliegue con Docker**
- Configuración docker-compose lista
- Imágenes multi-stage optimizadas
- Listo para producción

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

### Opción 1: Docker (Recomendado)

**Prerrequisitos:**
- Docker instalado
- Docker Compose

**Ejecutar:**
```bash
cd Source
docker compose up --build
```

La aplicación estará disponible en:
- **Frontend**: http://localhost (ej: http://localhost/trips)
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

Para más detalles sobre Docker, ver [DOCKER-README.md](DOCKER-README.md)

### Opción 2: Ejecución Manual

#### Prerrequisitos
- .NET 8 SDK
- Node.js 20+
- Git

#### Backend
#### Backend

1. **Navegar al backend**
```bash
cd Source/Backend
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Ejecutar la aplicación**
```bash
dotnet run --project TransportControl.API
```

4. **Acceder al backend**
   - API: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`

#### Frontend

1. **Navegar al frontend**
```bash
cd Source/Frontend
```

2. **Instalar dependencias**
```bash
npm install
```

3. **Ejecutar en desarrollo**
```bash
ng serve
```

4. **Acceder al frontend**
   - URL: `http://localhost:4200`

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

### Funcionalidades Implementadas
- ✅ Registro y gestión completa de viajes
- ✅ Administración de operadores de transporte
- ✅ Gestión de catálogo de lugares (origen/destino)
- ✅ Interfaz de usuario responsive con Angular Material
- ✅ API REST documentada con Swagger
- ✅ Despliegue con Docker y docker-compose
- ✅ Arquitectura limpia y escalable

### Información del Viaje
- **Origen y Destino**: Lugares de inicio y fin del viaje
- **Fechas programadas**: Inicio y fin planificados
- **Fechas reales**: Registro de tiempos efectivos
- **Operador asignado**: Conductor del viaje
- **Estados**: Programado, En Progreso, Completado, Cancelado
- **Distancias**: Estimadas y reales
- **Notas y vehículo**: Información adicional

## Base de Datos

El proyecto utiliza SQL Server con Code First de Entity Framework Core.

**Servidor**: somee.com (remoto)
**Estrategia**: Code First con migraciones automáticas

### Migraciones (si es necesario)
```bash
cd Source/Backend/Infrastructure
dotnet ef migrations add NombreMigracion --startup-project ../TransportControl.API
dotnet ef database update --startup-project ../TransportControl.API
```

## Tecnología Stack

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Base de datos
- **Swagger/OpenAPI** - Documentación de API
- **Clean Architecture** - Patrón arquitectónico

### Frontend
- **Angular 20** - Framework SPA
- **Angular Material** - Componentes UI
- **RxJS** - Programación reactiva
- **TypeScript** - Lenguaje tipado

### DevOps
- **Docker** - Contenedores
- **Docker Compose** - Orquestación
- **Nginx** - Servidor web y proxy reverso
- **GitHub** - Control de versiones

## Documentación Adicional

- [Documentación de Docker](DOCKER-README.md) - Guía completa de despliegue con Docker
- [API Endpoints](http://localhost:5000/swagger) - Documentación interactiva de la API

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.