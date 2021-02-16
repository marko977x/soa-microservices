FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
COPY . .
RUN ["dotnet", "restore"]
COPY . ./
RUN dotnet build "DeviceMicroservice.csproj" -c Release -o /app/build
RUN dotnet publish "DeviceMicroservice.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY --from=base /app/publish .
ENTRYPOINT ["dotnet", "DeviceMicroservice.dll"]

# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# WORKDIR /device-microservice/src
# COPY . .
# RUN dotnet restore "DeviceMicroservice.csproj"
# RUN dotnet build "DeviceMicroservice.csproj" -c Release -o /device-microservice/build
# ENTRYPOINT ["dotnet", "DeviceMicroservice.dll"]

# FROM build AS publish
# RUN dotnet publish "DeviceMicroservice.csproj" -c Release -o /device-microservice/publish

# FROM base AS final
# WORKDIR /device-microservice
# COPY --from=publish /device-microservice/publish .
# ENTRYPOINT ["dotnet", "DeviceMicroservice.dll"]