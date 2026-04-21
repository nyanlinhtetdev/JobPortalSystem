# JobPortalSystem

The **Job Portal System** is a RESTful API backend built with **ASP.NET Core 8**. It serves as a platform connecting Employers and Job Seekers. The system allows employers to manage job postings and track applications, while job seekers can search, save, and apply for opportunities.

## Tech Stack
- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core & Dapper (Hybrid approach for optimized data access)
- **Authentication:** JWT with Refresh Token support
- **Documentation:** Swagger (OpenAPI)

## Key Features
- User Authentication (JWT + Refresh Token)
- Role-based Authorization (Job Seeker / Employer)
- Job Management (Create, Update, Delete)
- Job Search with Filtering (Title, Location, Salary)
- Job Application System
- Save Jobs Feature

## Project Structure
- **JobPortalSystem.Api**: The core API project containing Controllers, DTOs, and Middlewares.
- **JobPortal.Database**: A Class Library project for the data access layer, housing the EF Core `AppDbContext`, database models, and migrations.

## Database Table Structures

### Users

```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```
### Jobs

```sql
CREATE TABLE Jobs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Location NVARCHAR(255),
    Salary INT,

    EmployerId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,

    FOREIGN KEY (EmployerId) REFERENCES Users(Id)
);
```
### JobApplications

```sql
CREATE TABLE JobApplications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),

    JobId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,

    Status NVARCHAR(50) DEFAULT 'Pending',
    AppliedAt DATETIME2 DEFAULT GETUTCDATE(),

    FOREIGN KEY (JobId) REFERENCES Jobs(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

```

### SavedJobs

```sql
CREATE TABLE SavedJobs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),

    JobId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,

    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),

    FOREIGN KEY (JobId) REFERENCES Jobs(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

### RefreshTokens

```sql
CREATE TABLE RefreshTokens (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),

    UserId UNIQUEIDENTIFIER NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    ExpiryDate DATETIME2 NOT NULL,
    IsRevoked BIT DEFAULT 0,

    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```
## Database First Approach

This project uses a **Database-First approach** with Entity Framework Core.

- The database schema is created and managed directly in SQL Server
- EF Core models and `DbContext` are generated using the `Scaffold-DbContext` command
- A hybrid approach is used: **EF Core + Dapper** for optimized data access

### Scaffold Command

```bash
dotnet ef dbcontext scaffold "Server=.;Database=JobPortalDB;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f
```

## API Testing (Postman)

You can test the API using the provided Postman collection:

1. Import the collection from `/Postman/Job Portal API.postman_collection.json`
2. Import the environment file
3. Set `baseUrl` to your local server
4. Start with **Register → Login**
5. Token will be automatically saved for authorized requests
