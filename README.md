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

# Database Table Structures

## Users

```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
```

## Jobs

```sql
CREATE TABLE Jobs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Location NVARCHAR(255),
    Salary INT,

    EmployerId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),

    FOREIGN KEY (EmployerId) REFERENCES Users(Id)
);
```
