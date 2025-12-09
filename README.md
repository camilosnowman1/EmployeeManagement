# Employee Management System - TalentoPlus S.A.S.

A comprehensive .NET 8 Employee Management System built with Clean Architecture, featuring AI-powered analytics, PDF generation, Excel import, and automated email notifications.

## 🏗️ Architecture

This solution follows **Clean Architecture** principles with clear separation of concerns:

- **Domain**: Core business entities and interfaces
- **Application**: Business logic, CQRS commands/queries, DTOs, validators
- **Infrastructure**: Data access, external services (Email, PDF, Excel, AI)
- **WebAPI**: REST API with JWT authentication
- **WebApp**: Razor Pages admin interface with ASP.NET Core Identity
- **Tests**: Unit and integration tests

## ✨ Features

### Core Functionality
- ✅ **CRUD Operations**: Complete employee management
- ✅ **Pagination**: 10-20 items per page
- ✅ **Authentication & Authorization**:
  - ASP.NET Core Identity (WebApp - cookie-based)
  - JWT Bearer (WebAPI)
- ✅ **PDF Generation**: Employee resume ("Hoja de Vida") using QuestPDF
- ✅ **Excel Import**: Bulk employee data import with validation
- ✅ **Email Notifications**: SMTP welcome emails on registration

### Advanced Features
- ✅ **AI Dashboard**: Gemini AI-powered natural language queries
- ✅ **Statistics Cards**: Total employees, on vacation, by department
- ✅ **Public API Endpoints**:
  - `GET /api/departments` - List all departments
  - `POST /api/auth/register` - Employee self-registration
  - `POST /api/auth/login` - JWT authentication
- ✅ **Protected API Endpoints** (JWT required):
  - `GET /api/employees/me` - Get own employee info
  - `GET /api/employees/me/pdf` - Download own resume PDF

### DevOps & Quality
- ✅ **Docker Support**: Full containerization with docker-compose
- ✅ **Automated Tests**: Unit tests (FluentValidation) + Integration tests (API)
- ✅ **Environment Variables**: Secure configuration management
- ✅ **Cloud Deployment**: Configured for Clever Cloud with PostgreSQL

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL (local or cloud)

### Running with Docker (Recommended)

1. **Clone the repository**
```bash
git clone https://github.com/camilosnowman1/EmployeeManagement.git
cd EmployeeManagement
```

2. **Configure environment variables**
```bash
cp .env.example .env
# Edit .env and set your POSTGRESQL_ADDON_URI
```

3. **Start the application**
```bash
docker compose up --build
```

4. **Access the applications**
- **WebAPI (Swagger)**: http://localhost:5001/swagger
- **WebApp (Admin Panel)**: http://localhost:5002

### Running Locally (Without Docker)

1. **Set up PostgreSQL connection**
```bash
export POSTGRESQL_ADDON_URI="postgres://user:password@localhost:5432/EmployeeDB"
```

2. **Run migrations**
```bash
cd WebAPI
dotnet ef database update
```

3. **Start WebAPI**
```bash
cd WebAPI
dotnet run
```

4. **Start WebApp** (in another terminal)
```bash
cd WebApp
dotnet run
```

## 🔐 Default Credentials

### Admin User (WebApp)
- **Email**: `admin@talentoplus.com`
- **Password**: `Admin@123`

### Test Employee (API)
Register via `POST /api/auth/register` or use the WebApp to create employees.

## 🧪 Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "FullyQualifiedName~UnitTests"

# Run only integration tests (requires database)
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## 📝 Environment Variables

### Required
- `POSTGRESQL_ADDON_URI`: PostgreSQL connection string (format: `postgres://user:pass@host:port/db`)

### Optional
- `Jwt:Key`: JWT signing key (default provided)
- `Jwt:Issuer`: JWT issuer (default: `EmployeeAPI`)
- `Jwt:Audience`: JWT audience (default: `EmployeeAPI`)
- `GEMINI_API_KEY`: Google Gemini API key for AI features
- `EmailSettings:SmtpServer`: SMTP server for email notifications
- `EmailSettings:SmtpPort`: SMTP port
- `EmailSettings:SmtpUser`: SMTP username
- `EmailSettings:SmtpPass`: SMTP password
- `EmailSettings:FromEmail`: Sender email address
- `EmailSettings:FromName`: Sender display name

## 🌐 API Endpoints

### Public Endpoints
| Method | Endpoint             | Description             |
| ------ | -------------------- | ----------------------- |
| GET    | `/api/departments`   | List all departments    |
| POST   | `/api/auth/register` | Register new employee   |
| POST   | `/api/auth/login`    | Login and get JWT token |

### Protected Endpoints (JWT Required)
| Method | Endpoint                | Description                  |
| ------ | ----------------------- | ---------------------------- |
| GET    | `/api/employees`        | List employees (paginated)   |
| GET    | `/api/employees/{id}`   | Get employee by ID           |
| POST   | `/api/employees`        | Create employee              |
| PUT    | `/api/employees/{id}`   | Update employee              |
| DELETE | `/api/employees/{id}`   | Delete employee              |
| GET    | `/api/employees/me`     | Get own employee info        |
| GET    | `/api/employees/me/pdf` | Download own resume PDF      |
| GET    | `/api/dashboard/stats`  | Get dashboard statistics     |
| POST   | `/api/ai/query`         | Query AI about employee data |

## 🤖 AI Dashboard Usage

1. Navigate to the Dashboard in the WebApp
2. Ask questions in natural language, for example:
   - "How many employees are in the Technology department?"
   - "How many employees are on vacation?"
   - "How many developers do we have?"

**Note**: Requires `GEMINI_API_KEY` environment variable to be set.

## 📦 Project Structure

```
EmployeeManagement/
├── Domain/                 # Core entities and interfaces
├── Application/            # Business logic, CQRS, DTOs
├── Infrastructure/         # Data access, services
├── WebAPI/                 # REST API
├── WebApp/                 # Razor Pages admin interface
├── Tests/                  # Unit and integration tests
├── Tools/DbVerifier/       # Database verification tool
├── docker-compose.yml      # Docker orchestration
└── README.md               # This file
```

## 🚢 Deployment to Clever Cloud

1. **Create a .NET application** in Clever Cloud
2. **Link PostgreSQL addon** to your application
3. **Set environment variables**:
   - `CC_DOTNET_PROJECT` = `WebAPI/WebAPI.csproj` (for API)
   - `CC_DOTNET_PROJECT` = `WebApp/WebApp.csproj` (for WebApp)
4. **Push to GitHub** - Clever Cloud will auto-deploy
5. **Run migrations** via Clever Cloud console or locally:
   ```bash
   dotnet ef database update --project WebAPI
   ```

## 📄 License

This project is part of a technical assessment for TalentoPlus S.A.S.

## 👨‍💻 Author

Developed by Camilo - .NET Developer Assessment

---

**Built with**: .NET 8, PostgreSQL, Entity Framework Core, ASP.NET Core Identity, JWT, QuestPDF, ExcelDataReader, MailKit, Google Gemini AI, Docker
