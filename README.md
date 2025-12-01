# Project Architecture

## Architecture

The projects are divided into 3 layers:

- **API** - Web API with controllers and Kafka Consumer
- **Domain** - Domain entities
- **Infrastructure** - Repository implementations and external clients

## Functionality

- Receiving commands from Kafka
- Updating device statuses in the database
- Sending commands to physical devices via HTTP
- API for retrieving and updating device statuses

## Technologies

- .NET 8.0
- Dapper for database access
- Refit for HTTP clients
- Confluent.Kafka for working with Kafka
- PostgreSQL for data storage