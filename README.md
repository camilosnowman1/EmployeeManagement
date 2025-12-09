# Employee Management System - TalentoPlus S.A.S.

## 📋 Descripción del Proyecto

Sistema completo de gestión de empleados desarrollado con .NET 8, ASP.NET Core, PostgreSQL y Clean Architecture. Incluye aplicación web para administradores, API REST con JWT, importación de Excel, generación de PDFs, y dashboard con IA (Gemini).

## 🚀 Quick Start

### Requisitos Previos
- Docker y Docker Compose instalados
- Puerto 5001 (WebAPI), 5002 (WebApp), 5432 (PostgreSQL) disponibles

### Ejecución con Docker

```bash
# 1. Clonar el repositorio
git clone https://github.com/camilosnowman1/EmployeeManagement.git
cd EmployeeManagement

# 2. Ejecutar migraciones (PRIMERA VEZ)
docker compose up -d db
sleep 10
cd WebAPI
dotnet ef database update --connection "Host=localhost;Port=5432;Database=EmployeeDB;Username=postgres;Password=admin"
cd ..

# 3. Iniciar todos los servicios
docker compose up -d

# 4. Verificar que todo esté corriendo
docker compose ps
```

### Acceso a las Aplicaciones

- **WebApp (Admin)**: http://localhost:5002
- **WebAPI (Swagger)**: http://localhost:5001/swagger
- **PostgreSQL**: localhost:5432

## 🔐 Credenciales de Acceso

### Administrador Web (ASP.NET Core Identity)
**IMPORTANTE**: En el primer acceso, debes registrarte en http://localhost:5002/Identity/Account/Register

- **Email**: admin@talento plus.com (o el que registres)
- **Password**: Admin123! (mínimo 6 caracteres, 1 mayúscula, 1 número)

### Base de Datos PostgreSQL
- **Host**: localhost (o `db` dentro de Docker)
- **Port**: 5432
- **Database**: EmployeeDB
- **Username**: postgres
- **Password**: admin

## 📊 Funcionalidades Implementadas

### ✅ 1. Aplicación Web (Administrador)
- **Autenticación**: ASP.NET Core Identity
- **CRUD Empleados**: Crear, editar, listar, eliminar
- **Importación Excel**: Subir archivo .xlsx con empleados
- **Generación PDF**: Hoja de vida de cada empleado
- **Dashboard**: Estadísticas + IA para consultas en lenguaje natural

### ✅ 2. API REST
#### Endpoints Públicos
- `GET /api/departments` - Listar departamentos

#### Endpoints Protegidos (requieren JWT)
- `GET /api/employees` - Listar empleados (paginado)
- `GET /api/employees/{id}` - Obtener empleado por ID
- `POST /api/employees` - Crear empleado
- `PUT /api/employees/{id}` - Actualizar empleado
- `DELETE /api/employees/{id}` - Eliminar empleado
- `GET /api/employees/me/pdf` - Descargar PDF propio
- `GET /api/dashboard/stats` - Estadísticas
- `POST /api/ai/query` - Consultas IA

### ✅ 3. Dashboard con IA (Gemini)
- **Tarjetas de Estadísticas**:
  - Total de empleados
  - Empleados en vacaciones
  - Empleados activos
- **Asistente IA**: Consultas en lenguaje natural
  - Ejemplo: "¿Cuántos desarrolladores tenemos?"
  - Ejemplo: "¿Cuántos empleados hay en Marketing?"

### ✅ 4. Pruebas Automatizadas
- **Unit Tests** (2):
  - `CreateEmployeeValidatorTests.Should_Have_Error_When_FirstName_Is_Empty`
  - `CreateEmployeeValidatorTests.Should_Not_Have_Error_When_Command_Is_Valid`
- **Integration Tests** (2):
  - `DepartmentsApiTests.GetDepartments_ReturnsSuccessStatusCode`
  - `DepartmentsApiTests.GetDepartments_ReturnsListOfStrings`

```bash
# Ejecutar tests
dotnet test
```

### ✅ 5. Arquitectura
- **Clean Architecture**: Domain, Application, Infrastructure, WebAPI, WebApp
- **Patrón Repositorio**: `IEmployeeRepository`
- **CQRS**: MediatR con Commands y Queries
- **Validación**: FluentValidation
- **Mapping**: AutoMapper

## 🔧 Variables de Entorno

### Docker Compose (ya configuradas)
```yaml
# WebAPI y WebApp
POSTGRESQL_ADDON_URI=Host=db;Port=5432;Database=EmployeeDB;Username=postgres;Password=admin;SSL Mode=Disable
ASPNETCORE_ENVIRONMENT=Development
GEMINI_API_KEY=AIzaSyD9thWTYxjEzPXYYCabaaA4OqmW44JDvIE

# WebApp adicional
ApiBaseUrl=http://webapi:8080
```

### Para Desarrollo Local (sin Docker)
```bash
export POSTGRESQL_ADDON_URI="postgres://postgres:admin@localhost:5432/EmployeeDB"
export GEMINI_API_KEY="AIzaSyD9thWTYxjEzPXYYCabaaA4OqmW44JDvIE"
```

## 📁 Estructura del Proyecto

```
EmployeeManagement/
├── Domain/                 # Entidades y contratos
│   ├── Entities/
│   └── Interfaces/
├── Application/            # Lógica de negocio (CQRS)
│   ├── DTOs/
│   ├── Employees/
│   │   ├── Commands/
│   │   └── Queries/
│   ├── Interfaces/
│   └── Mappings/
├── Infrastructure/         # Implementaciones
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
├── WebAPI/                 # API REST
│   └── Controllers/
├── WebApp/                 # Razor Pages
│   └── Pages/
├── Tests/                  # Pruebas
│   ├── UnitTests/
│   └── IntegrationTests/
└── docker-compose.yml
```

## 🎯 Uso del Sistema

### 1. Importar Empleados desde Excel
1. Ir a http://localhost:5002/Employees
2. Hacer clic en "Seleccionar archivo"
3. Subir el archivo `Empleados.xlsx`
4. Hacer clic en "Upload"
5. Los empleados se importarán automáticamente

### 2. Generar PDF de Empleado
1. En la lista de empleados, hacer clic en el botón azul "PDF"
2. Se descargará automáticamente el archivo `Employee_{ID}.pdf`

### 3. Usar el Dashboard con IA
1. Ir a http://localhost:5002/Dashboard
2. Ver las estadísticas en las tarjetas
3. En "AI Assistant", escribir una pregunta:
   - "¿Cuántos empleados trabajan en Tecnología?"
   - "¿Cuántos desarrolladores tenemos?"
4. Hacer clic en "Ask AI"
5. Ver la respuesta generada por Gemini

## ⚠️ Funcionalidades Pendientes

Por limitaciones de tiempo, las siguientes funcionalidades **NO están implementadas** pero están documentadas para futura implementación:

### 1. Autoregistro de Empleados (API Pública)
**Endpoint faltante**: `POST /api/employees/register`

**Implementación sugerida**:
```csharp
// En WebAPI/Controllers/EmployeesController.cs
[HttpPost("register")]
[AllowAnonymous]
public async Task<IActionResult> Register([FromBody] RegisterEmployeeCommand command)
{
    var result = await _mediator.Send(command);
    // Enviar email de bienvenida aquí
    return Ok(result);
}
```

### 2. Login de Empleados (JWT)
**Endpoint faltante**: `POST /api/auth/login`

**Implementación sugerida**:
```csharp
// Crear WebAPI/Controllers/AuthController.cs
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginCommand command)
{
    // Validar credenciales
    // Generar JWT token
    return Ok(new { token = "..." });
}
```

### 3. Consultar Información Propia
**Endpoint faltante**: `GET /api/employees/me`

**Implementación sugerida**:
```csharp
[HttpGet("me")]
[Authorize]
public async Task<IActionResult> GetMyInfo()
{
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var employee = await _repository.GetByEmailAsync(email);
    return Ok(employee);
}
```

### 4. Envío de Email SMTP
**Servicio**: `EmailService` existe pero no está configurado

**Configuración necesaria**:
```yaml
# En docker-compose.yml
- SMTP_HOST=smtp.gmail.com
- SMTP_PORT=587
- SMTP_USERNAME=tu-email@gmail.com
- SMTP_PASSWORD=tu-app-password
```

## 🐛 Problemas Conocidos

### 1. Paginación en WebApp
**Síntoma**: Al hacer clic en "Next" o en números de página, muestra los mismos empleados.

**Workaround**: Usar la API directamente:
```bash
curl "http://localhost:5001/api/employees?page=2&pageSize=10"
```

**Causa**: Posible problema de cache en el navegador o en Razor Pages.

### 2. IA Gemini
**Síntoma**: A veces retorna "BadRequest" o "Error calling Gemini API".

**Solución**: 
- Verificar que `GEMINI_API_KEY` esté configurada
- Reiniciar contenedores: `docker compose restart`
- La API Key incluida tiene límites de uso gratuitos

## 🚢 Despliegue en Clever Cloud

### WebAPI
1. Crear aplicación .NET en Clever Cloud
2. Vincular addon PostgreSQL
3. Configurar variables de entorno:
   ```
   CC_DOTNET_PROJECT=WebAPI/WebAPI.csproj
   GEMINI_API_KEY=tu-key
   ```
4. Push a GitHub (auto-deploy)

### WebApp
1. Crear aplicación .NET separada
2. Vincular mismo addon PostgreSQL
3. Configurar variables:
   ```
   CC_DOTNET_PROJECT=WebApp/WebApp.csproj
   ApiBaseUrl=https://tu-webapi.cleverapps.io
   GEMINI_API_KEY=tu-key
   ```
4. Push a GitHub

## 📝 Comandos Útiles

```bash
# Ver logs
docker compose logs -f webapi
docker compose logs -f webapp

# Reiniciar servicios
docker compose restart webapi webapp

# Detener todo
docker compose down

# Limpiar base de datos (CUIDADO: borra datos)
docker compose down -v

# Ejecutar migraciones
cd WebAPI
dotnet ef database update

# Ejecutar tests
dotnet test

# Ver empleados en BD
docker exec -it employeemanagement-db-1 psql -U postgres -d EmployeeDB -c "SELECT COUNT(*) FROM \"Employees\";"
```

## 📧 Contacto y Soporte

- **Repositorio**: https://github.com/camilosnowman1/EmployeeManagement
- **Desarrollador**: Camilo (camilosnowman1)
- **Tecnologías**: .NET 8, PostgreSQL, Docker, Gemini AI

## 📄 Licencia

Este proyecto fue desarrollado como prueba técnica para TalentoPlus S.A.S.

---

**Nota**: Este README documenta el estado actual del proyecto. Las funcionalidades marcadas como "Pendientes" están diseñadas pero no implementadas por limitaciones de tiempo. El código base está preparado para su fácil implementación siguiendo los patrones ya establecidos.
