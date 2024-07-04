API
  dotnet watch --no-hot-reload
  dotnet run --no-hot-reload

client
  ng serve


migration
  dotnet ef migrations add ExtendedUserEntity
  dotnet ef database update
  dotnet ef database drop
