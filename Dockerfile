# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY /out .
ENTRYPOINT ["dotnet", "HealthAPI.dll"]