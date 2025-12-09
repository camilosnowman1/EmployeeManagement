# Employee Management System

A Clean Architecture .NET 8 Web API for managing Employee data, configured for PostgreSQL on Clever Cloud.

## Project Structure

The solution follows the Clean Architecture principles with four main layers:

1.  **Domain**: Contains the core business logic, Entities (`Employee`), and Interfaces (`IEmployeeRepository`). This layer has no dependencies.
2.  **Application**: Contains the application logic, CQRS (Commands and Queries), DTOs, Validators, and AutoMapper profiles. It depends on the Domain layer.
3.  **Infrastructure**: Contains the implementation of interfaces (e.g., `EmployeeRepository`), Database Context (`AppDbContext`), and EF Core configurations. It depends on the Domain and Application layers.
4.  **WebAPI**: The entry point of the application. It contains Controllers and Dependency Injection configuration. It depends on the Application and Infrastructure layers.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/) (or Clever Cloud account)

## Configuration (Clever Cloud PostgreSQL)

The application uses Environment Variables to configure the database connection. This ensures security and flexibility for deployment.

You must set the following environment variables before running the application:

- `DB_HOST`: The hostname of your PostgreSQL server (e.g., `postgresql-123.clever-cloud.com`).
- `DB_PORT`: The port number (default `5432`).
- `DB_NAME`: The name of the database.
- `DB_USER`: The username.
- `DB_PASSWORD`: The password.
- `DB_SSLMODE`: The SSL mode (e.g., `Require`, `Prefer`).

### Example Connection String Construction
The application constructs the connection string dynamically:
`Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};SslMode=${DB_SSLMODE}`

## How to Run Locally

1.  **Clone the repository**:
    ```bash
    git clone <repository-url>
    cd EmployeeManagement
    ```

2.  **Set Environment Variables**:
    (Linux/macOS)
    ```bash
    export DB_HOST=localhost
    export DB_PORT=5432
    export DB_NAME=employee_db
    export DB_USER=postgres
    export DB_PASSWORD=your_password
    export DB_SSLMODE=Prefer
    ```
    (Windows PowerShell)
    ```powershell
    $env:DB_HOST="localhost"
    ...
    ```

3.  **Apply Migrations**:
    ```bash
    dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebAPI
    dotnet ef database update --project Infrastructure --startup-project WebAPI
    ```

4.  **Run the API**:
    ```bash
    cd WebAPI
    dotnet run
    ```

5.  **Access Swagger UI**:
    Open `http://localhost:5000/swagger` in your browser.

## API Endpoints

| Method   | Endpoint              | Description            |
| :------- | :-------------------- | :--------------------- |
| `GET`    | `/api/employees`      | Get all employees.     |
| `GET`    | `/api/employees/{id}` | Get employee by ID.    |
| `POST`   | `/api/employees`      | Create a new employee. |
| `PUT`    | `/api/employees/{id}` | Update an employee.    |
| `DELETE` | `/api/employees/{id}` | Delete an employee.    |

## Deployment to Clever Cloud

1.  Create a **.NET** application on Clever Cloud.
2.  Add a **PostgreSQL** add-on.
3.  Link the PostgreSQL add-on to your application. Clever Cloud automatically injects environment variables, but you might need to map them if they differ from the expected names (e.g., `POSTGRESQL_ADDON_HOST` -> `DB_HOST`).
4.  Push your code to the Clever Cloud Git remote.
