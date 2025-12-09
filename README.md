# Employee Management Solution

A Clean Architecture .NET 8 solution featuring a **Web API** and a **Razor Pages Web Application**, configured for PostgreSQL on Clever Cloud.

## Project Structure

The solution follows Clean Architecture principles:

1.  **Domain**: Core business logic, Entities (`Employee`), and Interfaces.
2.  **Application**: Use Cases (CQRS with MediatR), DTOs, Validators.
3.  **Infrastructure**: Data Access (EF Core, PostgreSQL), Repositories.
4.  **WebAPI**: RESTful API endpoints.
5.  **WebApp**: Razor Pages Web Application (User Interface).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/) (or Clever Cloud account)

## Configuration

The application supports two ways to configure the database:

1.  **Clever Cloud Auto-Configuration**:
    The application automatically detects the `POSTGRESQL_ADDON_URI` environment variable provided by Clever Cloud. **No manual configuration is needed** if you link a PostgreSQL add-on.

2.  **Manual Configuration** (e.g., for local development):
    Set the following environment variables:
    - `DB_HOST`: Hostname
    - `DB_PORT`: Port (default `5432`)
    - `DB_NAME`: Database Name
    - `DB_USER`: Username
    - `DB_PASSWORD`: Password
    - `DB_SSLMODE`: SSL Mode (e.g., `Prefer`)

## How to Run

1.  **Clone the repository**:
    ```bash
    git clone <repository-url>
    cd EmployeeManagement
    ```

2.  **Apply Migrations**:
    ```bash
    dotnet ef database update --project Infrastructure --startup-project WebAPI
    ```

3.  **Run the Web Application (UI)**:
    ```bash
    cd WebApp
    dotnet run
    ```
    Access at `http://localhost:5000` (or port shown).

4.  **Run the API (Backend)**:
    ```bash
    cd WebAPI
    dotnet run
    ```
    Access Swagger at `http://localhost:5000/swagger`.

## Deployment to Clever Cloud

1.  Create a **.NET** application on Clever Cloud.
2.  Add a **PostgreSQL** add-on and link it.
3.  **Important**: Since there are two executable projects (`WebAPI` and `WebApp`), you need to specify which one to run in the Clever Cloud settings (Environment Variables):
    - `CC_DOTNET_PROJECT_FILE=WebApp/WebApp.csproj` (to deploy the UI)
    - OR `CC_DOTNET_PROJECT_FILE=WebAPI/WebAPI.csproj` (to deploy the API)
4.  Push your code to the Clever Cloud remote.
