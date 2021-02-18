#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
WORKDIR /app
COPY . .
RUN ["dotnet", "restore"]
COPY . ./
RUN dotnet build "DataMicroservice.csproj" -c Release -o /app/build
RUN dotnet publish "DataMicroservice.csproj" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY --from=base /app/publish .
ENTRYPOINT ["dotnet", "DataMicroservice.dll"]


# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# EXPOSE 5000
# WORKDIR /data-microservice

# FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

# WORKDIR /data-microservice/src
# COPY . .
# RUN dotnet restore "DataMicroservice.csproj"
# RUN dotnet build "DataMicroservice.csproj" -c Release -o /data-microservice/build
# ENTRYPOINT ["dotnet", "DataMicroservice.dll"]

# FROM build AS publish
# RUN dotnet publish "DataMicroservice.csproj" -c Release -o /data-microservice/publish

# FROM base AS final
# WORKDIR /data-microservice
# COPY --from=publish /data-microservice/publish .
# ENTRYPOINT ["dotnet", "DataMicroservice.dll"]