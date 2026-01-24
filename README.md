# PCM - Pickleball Club Management System

A full-stack web application for managing a pickleball club, built with ASP.NET Core Web API and Vue.js.

## Features

- User authentication and authorization (JWT)
- Member management
- Court booking system
- Match scheduling and tracking
- Challenge system
- News and notifications
- Activity logging
- Transaction management

## Tech Stack

### Backend
- ASP.NET Core 9.0 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- ASP.NET Identity

### Frontend
- Vue.js 3
- TypeScript
- Vue Router
- Pinia (State Management)
- Axios
- Vite

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+
- SQL Server

### Backend Setup
1. Navigate to the backend directory:
   ```bash
   cd PCM.Api
   ```

2. Restore packages:
   ```bash
   dotnet restore
   ```

3. Update the connection string in `appsettings.json` if needed.

4. Run database migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd PCM.Frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run the development server:
   ```bash
   npm run dev
   ```

The frontend will be available at `http://localhost:5173`.

## API Documentation

Once the backend is running, visit `https://localhost:5001/swagger` for API documentation.

## Database Schema

The application uses Entity Framework Code-First approach. The database includes tables for:
- Members
- Courts
- Bookings
- Matches
- Challenges
- Transactions
- Activity Logs
- And more...

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.