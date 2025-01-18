# DistributedLoggingSystem
Distributed Logging System

The Distributed Logging System is a robust and scalable solution designed to handle logging across multiple services with support for dynamic backend configuration. It provides flexibility to log entries into one of the following backends based on configuration:

Database
Local File System
Amazon S3-Compatible Storage
Message Queue

Features
Dynamic logging backend selection using appsettings.json.
Easy integration with .NET Core dependency injection (IoC).
Queryable log retrieval with filtering options.
Modular repository and factory pattern for extensibility.
Example implementations for database logging, local file logging, S3-compatible storage, and message queues.
Prerequisites
.NET Core SDK (version 6.0 or later)
Database: Microsoft SQL Server or a compatible database for database logging.
Optional: Access to an S3-compatible storage or message queue setup.
Configuration
Update the appsettings.json

ConnectionStrings with your database server

LoggingBackend
Backend: Determines the active logging mechanism (file, database, s3, or messagequeue).
DefaultConnection: Connection string for your database.
FilePath: Path to the log file when using local file logging.
S3Url and BucketName: S3 configuration for cloud storage.

Database Migration and Setup
To enable database logging, run the following commands:

Add Migration:
dotnet ef migrations add Addlog

Update Database:
dotnet ef database update

This will create the necessary schema for storing logs in the database.

Usage
Dependency Injection
The backend for logging is dynamically chosen at runtime based on the LoggingBackend:Backend key in appsettings.json. The implementation uses a factory pattern, and all backends are registered with the IoC container.

API Endpoints
Add Log:
POST /v1/logs
Adds a new log entry.

Request Body:
{
  "Service": "MyService",
  "Level": "Information",
  "Message": "This is a log message.",
  "Timestamp": "2025-01-01T12:00:00"
}

Retrieve Logs:
GET /v1/logs
Retrieves logs with optional query filters.

Query Parameters:

service (optional): Filter logs by service name.
level (optional): Filter logs by level (e.g., Information, Error).
startTime (optional): Filter logs after a specific timestamp.
endTime (optional): Filter logs before a specific timestamp.
Example:
GET /v1/logs?service=MyService&level=Error&startTime=2025-01-01T00:00:00&endTime=2025-01-02T00:00:00

Backend Implementations
1. Database Backend
Stores logs in the configured SQL Server database. Ensure the connection string in appsettings.json points to a valid database.

2. File Backend
Logs are stored in a text file specified by LoggingBackend:FilePath. Each log entry is structured for easy querying, and log files are named based on timestamp and service (e.g., logs-2025-01-19-MyService.txt).

3. S3-Compatible Storage Backend
Logs are uploaded as files to an S3-compatible bucket. Ensure S3Url and BucketName are configured in appsettings.json.

4. Message Queue Backend
Logs are sent as messages to a message queue. Implement your message queue provider (e.g., RabbitMQ, Kafka) for full integration.
