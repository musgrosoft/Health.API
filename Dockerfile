FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

RUN mkdir /app/HealthAPI/
COPY src/HealthAPI/*.csproj /app/HealthAPI/
WORKDIR /app/HealthAPI/
RUN dotnet restore

RUN mkdir /app/Migrators/
COPY src/Migrators/*.csproj /app/Migrators/
WORKDIR /app/Migrators/
RUN dotnet restore

RUN mkdir /app/Repositories/
COPY src/Repositories/*.csproj /app/Repositories/
WORKDIR /app/Repositories/
RUN dotnet restore

RUN mkdir /app/Services/
COPY src/Services/*.csproj /app/Services/
WORKDIR /app/Services/
RUN dotnet restore

RUN mkdir /app/Utils/
COPY src/Utils/*.csproj /app/Utils/
WORKDIR /app/Utils/
RUN dotnet restore

COPY src/Migrators/. /app/Migrators/
COPY src/Repositories/. /app/Repositories/
COPY src/Services/. /app/Services/
COPY src/Utils/. /app/Utils/
COPY src/HealthAPI/. /app/HealthAPI/

RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/HealthAPI/out .
ENTRYPOINT ["dotnet", "HealthAPI.dll"]