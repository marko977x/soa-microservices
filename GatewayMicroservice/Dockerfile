# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS base
WORKDIR /app
COPY GatewayMicroservice.csproj .
RUN ["dotnet", "restore"]
COPY . ./
RUN dotnet build "GatewayMicroservice.csproj" -c Release -o /app/build
RUN dotnet publish "GatewayMicroservice.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
COPY --from=base /app/publish .
ENTRYPOINT ["dotnet", "GatewayMicroservice.dll"]

# FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
# WORKDIR /src
# COPY ["GatewayMicroservice/GatewayMicroservice.csproj", "GatewayMicroservice/"]
# RUN dotnet restore "GatewayMicroservice/GatewayMicroservice.csproj"
# COPY . .
# WORKDIR "/src/GatewayMicroservice"
# RUN dotnet build "GatewayMicroservice.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "GatewayMicroservice.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "GatewayMicroservice.dll"]