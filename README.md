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
dotnet ef migrations add Initial
dotnet ef database update
```
Reset migrations:
```bash
dotnet ef database drop --force
dotnet ef migrations remove
```
