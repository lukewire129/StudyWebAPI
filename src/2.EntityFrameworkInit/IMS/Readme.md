```
dotnet new webapi -o IMS


# EF Core ..
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.1

# file create ~
cd database/context/
dotnet ef migrations add InitialCreate --project ..\..\IMS.csproj
```