# Transport Control System - Docker Setup

## 🚀 Inicio Rápido

### Prerrequisitos
- Docker instalado
- Docker Compose instalado

### Ejecutar la aplicación completa

```bash
# Clonar el repositorio
git clone https://github.com/gapalmas/transport-control-system.git
cd transport-control-system/Source

# Construir y ejecutar
docker compose up --build
```

La aplicación estará disponible en:
- **Frontend**: http://localhost (ej: http://localhost/trips)
- **Backend API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

### Comandos útiles

```bash
# Ejecutar en segundo plano
docker compose up -d

# Ver logs
docker compose logs -f

# Detener contenedores
docker compose down

# Reconstruir imágenes
docker compose build --no-cache

# Eliminar todo (contenedores, volúmenes, redes)
docker compose down -v
```

## 📦 Estructura Docker

```
Source/
├── docker-compose.yml          # Orquestación de servicios
├── .env                        # Variables de entorno
├── .env.example               # Template de variables
├── Backend/
│   ├── .dockerignore          # Archivos ignorados
│   └── TransportControl.API/
│       └── Dockerfile         # Imagen .NET 8
└── Frontend/
    ├── .dockerignore          # Archivos ignorados
    ├── Dockerfile             # Imagen Angular + nginx
    └── nginx.conf             # Configuración nginx
```

## 🏗️ Arquitectura

**Backend (transport-api)**
- .NET 8 Runtime
- Puerto: 5000 (HTTP)
- Build multi-stage para optimización
- Conexión a base de datos SQL Server remota

**Frontend (transport-frontend)**
- Angular 20 + nginx
- Puerto: 80
- Proxy reverso para API y Swagger
- Optimización de assets estáticos

**Network**
- Red bridge `transport-network`
- Comunicación interna entre contenedores

## 🐳 Subir a Docker Hub

```bash
# Login en Docker Hub
docker login

# Etiquetar imágenes
docker tag transport-api gapalmas/transport-api:latest
docker tag transport-frontend gapalmas/transport-frontend:latest

# Subir imágenes
docker push gapalmas/transport-api:latest
docker push gapalmas/transport-frontend:latest
```

## 🔧 Configuración

### Variables de entorno (.env)
```env
DB_CONNECTION_STRING=your_database_connection_string
```

### Para producción
1. Actualizar `.env` con credenciales reales
2. Configurar HTTPS con certificados SSL
3. Ajustar límites de recursos en docker-compose.yml

## ⚡ Optimizaciones

- **Multi-stage builds**: Reduce tamaño de imagen final
- **Gzip compression**: Compresión de assets en nginx
- **Cache layers**: Docker cache para builds más rápidos
- **Healthchecks**: Monitoreo automático de salud de contenedores

## 🎯 Para la entrevista técnica

El evaluador puede ejecutar todo el stack con un solo comando:

```bash
docker compose up
```

Esto iniciará:
1. Backend API en http://localhost:5000
2. Frontend en http://localhost (navegue a http://localhost/trips)
3. Documentación Swagger en http://localhost:5000/swagger

No se requiere configuración adicional. La base de datos ya está configurada en somee.com.
