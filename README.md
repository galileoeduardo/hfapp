# hfapp
Template ASP.NET Core MVC

#Isntall Requirements

.NET SDK 7.0

.NET Runtime 7.0

Entity Framework Core .NET Command-line Tools 7.0.12

If you have already installed entity framework, update to latest version:
```bash
dotnet tool update --global dotnet-efdotnet tool update --global dotnet-ef
```

#Setup Development Enviroment

Download project:
```bash
git clone https://github.com/galileoeduardo/hfapp.git
```

Run migrations:
```bash
cd hfapp\HFApp.WEB
dotnet ef migrations add hf --context HFDbContext
dotnet ef database update --context HFDbContext

dotnet ef migrations add auth --context AuthDbContext
dotnet ef database update --context AuthDbContext
```
Reset migrations:
```bash
dotnet ef database drop --context HFDbContext --force
dotnet ef migrations remove --context HFDbContext --force

dotnet ef database drop --context AuthDbContext --force
dotnet ef migrations remove --context AuthDbContext --force
```


API endpoints:

### Request
`GET /api/FileEntities`

### Request
`GET /api/FileEntities/{id}`

### Request
`GET /api/UserEntities`

### Request
`GET /api/UserEntities/{id}`