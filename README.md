RXAI Pharmacy Management System - Complete Overview
Project Overview
RXAI is a comprehensive pharmacy management system built with ASP.NET Core following clean code principles and onion architecture. The system provides a complete solution for modern pharmacies with features for prescription management, inventory control, analytics, and regulatory compliance.
Architecture
The application follows the Onion (Clean) Architecture pattern with four distinct layers:
Domain Layer (RXAI.Domain)
Core business entities with proper encapsulation
Repository interfaces
Domain service interfaces
No external dependencies
Application Layer (RXAI.Application)
DTOs (Data Transfer Objects)
Service interfaces and implementations
Business logic orchestration
Depends only on Domain layer
Infrastructure Layer (RXAI.Infrastructure)
Database context and configurations
Repository implementations
External service implementations (email, etc.)
Depends on Domain and Application layers
API Layer (RXAI.API)
Controllers and endpoints
Middleware configuration
API features (versioning, CORS, etc.)
Depends on all other layers
Key Features and Functions
1. Authentication & Authorization
User Management
Registration with email confirmation
Login with JWT token authentication
Password reset functionality
Refresh token mechanism
Account management
Role-Based Authorization
SuperDoctor role (full system access)
Doctor role (prescription management)
Pharmacist role (inventory management)
Patient role (limited access)
Permission-Based Authorization
Granular permissions for different operations
Permission policies for API endpoints
Custom authorization attributes
2. Security Features
Audit Logging
Comprehensive activity logging
Error logging
User action tracking
Regulatory compliance logging
Error Handling
Global exception handling middleware
Structured problem details responses
Environment-specific error details
Request/response logging
3. API Features
API Versioning
Support for multiple API versions
Version specification in URL, header, or media type
Default version fallback
CORS Configuration
Configurable allowed origins
Different policies for different endpoints
Support for credentials
Rate Limiting
Global rate limiting
Endpoint-specific rate limiting
Different limits for different user roles
4. Performance Features
Caching
In-memory caching
Distributed caching with Redis
Response caching with cache profiles
Cache invalidation strategies
Health Checks
Database connectivity monitoring
Memory usage monitoring
Disk space monitoring
Pharmacy-specific inventory health checks
5. Pharmacy-Specific Functions
Medication Inventory Management
Complete inventory tracking
Low stock alerts
Restocking functionality
Dispensing workflow
Expiry date tracking
Prescription Analytics
Prescription trends analysis
Medication usage patterns
Cost analysis
Patient adherence tracking
Drug Information
Drug interaction checking
Contraindications with medical conditions
Side effect information
Medication alternatives
Pharmacy Reporting
Sales reporting
Inventory valuation
Prescription trends
Regulatory compliance reporting
API Endpoints Overview
Authentication Endpoints
POST /api/v1/auth/register - Register a new user
POST /api/v1/auth/login - Authenticate a user
POST /api/v1/auth/refresh-token - Refresh an authentication token
POST /api/v1/auth/forgot-password - Initiate password reset
POST /api/v1/auth/reset-password - Complete password reset
POST /api/v1/auth/confirm-email - Confirm user email
User Management Endpoints
GET /api/v1/users - Get all users
GET /api/v1/users/{id} - Get user by ID
PUT /api/v1/users/{id} - Update user
DELETE /api/v1/users/{id} - Delete user
GET /api/v1/users/me - Get current user profile
Role Management Endpoints
GET /api/v1/roles - Get all roles
GET /api/v1/roles/{id} - Get role by ID
POST /api/v1/roles - Create a new role
PUT /api/v1/roles/{id} - Update role
DELETE /api/v1/roles/{id} - Delete role
GET /api/v1/roles/{roleName}/users - Get users in role
Medication Inventory Endpoints
GET /api/v1/medication-inventory - Get inventory
GET /api/v1/medication-inventory/{id} - Get inventory item
POST /api/v1/medication-inventory/{id}/restock - Restock inventory
POST /api/v1/medication-inventory/{id}/dispense - Dispense medication
GET /api/v1/medication-inventory/low-stock-alert - Get low stock alerts
Prescription Analytics Endpoints
GET /api/v1/prescription-analytics/trends - Get prescription trends
GET /api/v1/prescription-analytics/drug-interactions - Get drug interactions
GET /api/v1/prescription-analytics/adherence - Get medication adherence
GET /api/v1/prescription-analytics/cost-analysis - Get cost analysis
Drug Interaction Endpoints
GET /api/v1/drug-interaction/check - Check drug interactions
GET /api/v1/drug-interaction/contraindications - Check contraindications
GET /api/v1/drug-interaction/side-effects - Get side effects
GET /api/v1/drug-interaction/alternatives - Get medication alternatives
Pharmacy Reporting Endpoints
GET /api/v1/pharmacy-reporting/sales - Get sales report
GET /api/v1/pharmacy-reporting/inventory-valuation - Get inventory valuation
GET /api/v1/pharmacy-reporting/prescription-trends - Get prescription trends
GET /api/v1/pharmacy-reporting/regulatory-compliance - Get compliance report
Health Check Endpoints
GET /api/v1/health - Get overall health status
GET /api/v1/health/ready - Get readiness status
GET /api/v1/health/live - Get liveness status
Tools and Technologies
ASP.NET Core - Web framework
Entity Framework Core - ORM for database access
SQL Server - Primary database
Redis - Distributed caching (optional)
JWT - Token-based authentication
Swagger/OpenAPI - API documentation
xUnit - Testing framework
Getting Started
Prerequisites
.NET 8.0 SDK or later
SQL Server (or compatible database)
Redis (optional, for distributed caching)
Setup Instructions
Clone the repository
Update the connection strings in appsettings.json
Create a solution file and ensure all projects target the correct .NET version:
dotnet new sln -n RXAI
dotnet sln add RXAI.Domain/RXAI.Domain.csproj
dotnet sln add RXAI.Application/RXAI.Application.csproj
dotnet sln add RXAI.Infrastructure/RXAI.Infrastructure.csproj
dotnet sln add RXAI.API/RXAI.API.csproj
Restore packages:
dotnet restore
Run database migrations:
dotnet ef migrations add InitialCreate --project RXAI.Infrastructure --startup-project RXAI.API
dotnet ef database update --project RXAI.Infrastructure --startup-project RXAI.API
Run the application:
dotnet run --project RXAI.API
Configuration
The application is configured through appsettings.json with the following key sections:
ConnectionStrings - Database and Redis connections
JwtSettings - Authentication token configuration
EmailSettings - Email service configuration
AllowedOrigins - CORS configuration
Caching - Cache configuration
HealthChecks - Health monitoring thresholds
Pharmacy - Pharmacy-specific settings
Troubleshooting
If you encounter issues running the project:
Ensure all projects target the same .NET version (update .csproj files)
Clean the solution by deleting bin and obj folders
Restore NuGet packages with dotnet restore
Check project references to ensure proper dependencies
Verify database connection string is correct
Run with detailed logging enabled for troubleshooting
This enhanced RXAI system provides a comprehensive solution for modern pharmacy management with robust security, performance, and pharmacy-specific features. The clean architecture ensures maintainability and testability, while the extensive API allows for integration with other systems.
Successfully completed all enhancements to the RXAI application and provided comprehensive documentation
