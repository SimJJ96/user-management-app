# User Management App

A full-stack .NET 8 and Angular 20 application with Docker support for API, frontend, and SQL Server database.

---

## 📋 Prerequisites

Make sure you have the following installed on your machine:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Node.js 20+ and npm](https://nodejs.org/)
- [Docker & Docker Compose](https://www.docker.com/)

---

## 🧱 Project Structure

- **backend**: API logic with EF Core migrations and Dockerfile
- **frontend**: Angular client with nginx Dockerfile
- **docker-compose.yml**: Runs SQL Server, API, and frontend in containers


## ⚙️ Ports

| Service    | Docker Port | Host Access URL                           |
|------------|-------------|-------------------------------------------|
| SQL Server | `1433`     | `localhost:1433` (for DB tools like SSMS)  |
| API        | `8080`     | `http://localhost:7020`                    |
| Frontend   | `80`       | `http://localhost:4200`                    |


## 🐳 Running the App via Docker

Build and run all services:

```bash
docker-compose down
docker-compose build
docker-compose up
```

🔁 Subsequent Runs:
After the initial setup, you can remove the migrate command in docker-compose.yml in backend to avoid reapplying migrations on every startup.
Update the command under the backend service to:

#### Initial:
```bash
command: ["dotnet", "UserManagement.API.dll", "migrate"]
```

### Subsequent:
```bash
command: ["dotnet", "UserManagement.API.dll"]
```
This ensures normal API execution without attempting to run database migrations again.

After the services start:
- ✅ Visit the Angular frontend: http://localhost:4200
- ✅ Test the API via Swagger: http://localhost:7020/swagger


## 🛠 Development Tips

### API
You can still run and debug the API locally via Visual Studio or CLI:

```bash
cd backend/UserManagement/UserManagement.API
dotnet run
```

If you're developing the API outside of Docker (e.g., using Visual Studio), make sure your connection string in `appsettings.json` uses your **local SQL Server**:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=UserManagementDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
````

### Angular
To run the frontend outside Docker:

```bash
cd frontend
npm install
ng serve
```
