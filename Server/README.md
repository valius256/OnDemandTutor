
# Migration
Ref: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet#net-standard-limitation

It's about the limitation of the EF Core with .NET. We use the workaround 1 to solve our issue.

1. Add new migration
```
	 dotnet ef migrations add {MigrationName} -p .\DataAccess 
```

2. Remove migration
```
	 dotnet ef migrations remove -p .\DataAccess 
```

3. Update database (for developer environment)
```
    dotnet ef database update -p .\DataAccess 
```
