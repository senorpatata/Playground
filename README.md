# Translator
## This solution is based on a C# Project for teaching C# at Vinfoval (where I work)
## Is also on a CRQS project by @ducmeit, adding Angular frontend
## For this project I've removed console applications, other unit testing
## In other words: It's kind of my playground. It's nothing formal, but I try to keep always the code clean.

### Instructions:

- It uses a MSQL database to store things! so have one local SQLExpress installed if you want to run locally
- Start up project with Translator.API if you don't want to use Dockerized version

### Structures:

- Translator.API: API Layer
- Translator.Data: Data Layer
- Translator.Domain: Models Layer
- Translator.Service: Application Layer
- Translator.MSTest: Testing

### Run MSSQL Docker:

```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Demo123456@' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest
```

### Run DB Migrations:

```
dotnet ef database update
```

### Run application and test with Swagger:

```
dotnet run
```

- Access to: localhost:5000 or localhost:5001 with path: /swagger

### Run with docker-compose
- Requirement:
  - Installed docker and docker-compose
  
```
docker-compose build
docker-compose up
```

### Turn off auto migrate
- Please comment this line in `Startup.cs`

```
app.UseAutoMigrateDatabase<CustomerDbContext>();
```
