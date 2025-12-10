# Employee Management System - TalentoPlus S.A.S.

## 📋 Project Description

A comprehensive employee management system developed with .NET 8, ASP.NET Core, PostgreSQL, and Clean Architecture. It includes a web application for administrators, a REST API with JWT, Excel import functionality, PDF generation, and an AI-powered dashboard using Gemini.

## 🚀 Quick Start

### Prerequisites
- Docker and Docker Compose installed
- Ports 5001 (WebAPI), 5002 (WebApp), and 5432 (PostgreSQL) must be available

### Running with Docker

```bash
# 1. Clone the repository
git clone https://github.com/camilosnowman1/EmployeeManagement.git
cd EmployeeManagement

# 2. Build and run all services
# This command will also automatically apply database migrations on the first run.
docker compose up --build -d

# 3. Verify that everything is running
docker compose ps
```

### Accessing the Applications

- **WebApp (Admin)**: http://localhost:5002
- **WebAPI (Swagger)**: http://localhost:5001/swagger
- **PostgreSQL**: localhost:5432

## 🔐 Access Credentials

### Web Administrator (ASP.NET Core Identity)
**IMPORTANT**: On your first visit, you must register an account at http://localhost:5002/Identity/Account/Register

- **Email**: admin@talentoplus.com (or the one you register)
- **Password**: Admin123! (minimum 6 characters, 1 uppercase, 1 number)

### PostgreSQL Database
- **Host**: localhost (or `db` within Docker)
- **Port**: 5432
- **Database**: EmployeeDB
- **Username**: postgres
- **Password**: admin

## 📊 Implemented Features

### ✅ 1. Web Application (Administrator)
- **Authentication**: ASP.NET Core Identity
- **Employee CRUD**: Create, edit, list, delete
- **Excel Import**: Upload an .xlsx file with employee data
- **PDF Generation**: Employee resume/profile sheet
- **Dashboard**: Statistics + AI for natural language queries

### ✅ 2. REST API
#### Public Endpoints
- `GET /api/departments` - List departments

#### Protected Endpoints (require JWT)
- `GET /api/employees` - List employees (paginated)
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create an employee
- `PUT /api/employees/{id}` - Update an employee
- `DELETE /api/employees/{id}` - Delete an employee
- `GET /api/employees/me/pdf` - Download your own PDF
- `GET /api/dashboard/stats` - Get dashboard statistics
- `POST /api/ai/query` - Make AI-powered queries

### ✅ 3. AI Dashboard (Gemini)
- **Statistic Cards**:
  - Total employees
  - Employees on vacation
  - Active employees
- **AI Assistant**: Natural language queries
  - Example: "How many developers do we have?"
  - Example: "How many employees are in Marketing?"

### ✅ 4. Automated Tests
- **Unit Tests** (2):
  - `CreateEmployeeValidatorTests.Should_Have_Error_When_FirstName_Is_Empty`
  - `CreateEmployeeValidatorTests.Should_Not_Have_Error_When_Command_Is_Valid`
- **Integration Tests** (2):
  - `DepartmentsApiTests.GetDepartments_ReturnsSuccessStatusCode`
  - `DepartmentsApiTests.GetDepartments_ReturnsListOfStrings`

```bash
# Run tests
dotnet test
```

### ✅ 5. Architecture
- **Clean Architecture**: Domain, Application, Infrastructure, WebAPI, WebApp
- **Repository Pattern**: `IEmployeeRepository`
- **CQRS**: MediatR with Commands and Queries
- **Validation**: FluentValidation
- **Mapping**: AutoMapper

## 🔧 Environment Variables

### Docker Compose (already configured)
```yaml
# WebAPI & WebApp
POSTGRESQL_ADDON_URI=Host=db;Port=5432;Database=EmployeeDB;Username=postgres;Password=admin;SSL Mode=Disable
ASPNETCORE_ENVIRONMENT=Development
GEMINI_API_KEY=AIzaSyD9thWTYxjEzPXYYCabaaA4OqmW44JDvIE

# Additional for WebApp
ApiBaseUrl=http://webapi:8080
```

### For Local Development (without Docker)
```bash
export POSTGRESQL_ADDON_URI="postgres://postgres:admin@localhost:5432/EmployeeDB"
export GEMINI_API_KEY="AIzaSyD9thWTYxjEzPXYYCabaaA4OqmW44JDvIE"
```

## 📁 Project Structure

```
EmployeeManagement/
├── Domain/                 # Entities and contracts
│   ├── Entities/
│   └── Interfaces/
├── Application/            # Business logic (CQRS)
│   ├── DTOs/
│   ├── Employees/
│   │   ├── Commands/
│   │   └── Queries/
│   ├── Interfaces/
│   └── Mappings/
├── Infrastructure/         # Implementations
│   ├── Persistence/
│   ├── Repositories/
│   └── Services/
├── WebAPI/                 # REST API
│   └── Controllers/
├── WebApp/                 # Razor Pages
│   └── Pages/
├── Tests/                  # Tests
│   ├── UnitTests/
│   └── IntegrationTests/
└── docker-compose.yml
```

## 🎯 System Usage

### 1. Import Employees from Excel
1. Go to http://localhost:5002/Employees
2. Click on "Choose file"
3. Upload the `Employees.xlsx` file
4. Click "Upload"
5. The employees will be imported automatically

### 2. Generate Employee PDF
1. In the employee list, click the blue "PDF" button
2. The `Employee_{ID}.pdf` file will be downloaded automatically

### 3. Use the AI Dashboard
1. Go to http://localhost:5002/Dashboard
2. View the statistics on the cards
3. In the "AI Assistant" text box, type a question:
   - "How many employees work in Technology?"
   - "How many developers do we have?"
4. Click "Ask AI"
5. See the response generated by Gemini

## ⚠️ Pending Features

Due to time constraints, the following features are **NOT implemented** but are documented for future implementation:

### 1. Employee Self-Registration (Public API)
**Missing Endpoint**: `POST /api/employees/register`

**Suggested Implementation**:
```csharp
// In WebAPI/Controllers/EmployeesController.cs
[HttpPost("register")]
[AllowAnonymous]
public async Task<IActionResult> Register([FromBody] RegisterEmployeeCommand command)
{
    var result = await _mediator.Send(command);
    // Send welcome email here
    return Ok(result);
}
```

### 2. Employee Login (JWT)
**Missing Endpoint**: `POST /api/auth/login`

**Suggested Implementation**:
```csharp
// Create WebAPI/Controllers/AuthController.cs
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginCommand command)
{
    // Validate credentials
    // Generate JWT token
    return Ok(new { token = "..." });
}
```

### 3. Get Own Information
**Missing Endpoint**: `GET /api/employees/me`

**Suggested Implementation**:
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

### 4. SMTP Email Sending
**Service**: `EmailService` exists but is not configured

**Required Configuration**:
```yaml
# In docker-compose.yml
- SMTP_HOST=smtp.gmail.com
- SMTP_PORT=587
- SMTP_USERNAME=your-email@gmail.com
- SMTP_PASSWORD=your-app-password
```

## 🐛 Known Issues

### 1. Pagination in WebApp
**Symptom**: Clicking "Next" or page numbers shows the same employees.

**Workaround**: Use the API directly:
```bash
curl "http://localhost:5001/api/employees?page=2&pageSize=10"
```

**Cause**: Possible caching issue in the browser or Razor Pages.

### 2. Gemini AI
**Symptom**: Sometimes returns "BadRequest" or "Error calling Gemini API".

**Solution**: 
- Verify that `GEMINI_API_KEY` is configured
- Restart containers: `docker compose restart`
- The included API Key has free usage limits

## 🚢 Deployment to Clever Cloud

### WebAPI
1. Create a .NET application in Clever Cloud
2. Link the PostgreSQL addon
3. Configure environment variables:
   ```
   CC_DOTNET_PROJECT=WebAPI/WebAPI.csproj
   GEMINI_API_KEY=your-key
   ```
4. Push to GitHub (for auto-deploy)

### WebApp
1. Create a separate .NET application
2. Link the same PostgreSQL addon
3. Configure variables:
   ```
   CC_DOTNET_PROJECT=WebApp/WebApp.csproj
   ApiBaseUrl=https://your-webapi.cleverapps.io
   GEMINI_API_KEY=your-key
   ```
4. Push to GitHub

## 📝 Useful Commands

```bash
# View logs
docker compose logs -f webapi
docker compose logs -f webapp

# Restart services
docker compose restart webapi webapp

# Stop everything
docker compose down

# Clean up the database (WARNING: deletes data)
docker compose down -v

# Manually run migrations (not needed with the current docker-compose setup)
# cd WebAPI
# dotnet ef database update

# Run tests
dotnet test

# See employee count in DB
docker exec -it employeemanagement-db-1 psql -U postgres -d EmployeeDB -c "SELECT COUNT(*) FROM \"Employees\";"
```

## 📧 Contact and Support

- **Repository**: https://github.com/camilosnowman1/EmployeeManagement
- **Developer**: Camilo (camilosnowman1)
- **Technologies**: .NET 8, PostgreSQL, Docker, Gemini AI

## 📄 License

This project was developed as a technical test for TalentoPlus S.A.S.

---

**Note**: This README documents the current state of the project. Features marked as "Pending" are designed but not implemented due to time constraints. The codebase is prepared for their easy implementation following the established patterns.
