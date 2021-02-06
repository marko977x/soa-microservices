#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
EXPOSE 4000
WORKDIR /device-microservice

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /device-microservice/src
COPY . .
RUN dotnet restore "DeviceMicroservice.csproj"
RUN dotnet build "DeviceMicroservice.csproj" -c Release -o /device-microservice/build
ENTRYPOINT ["dotnet", "DeviceMicroservice.dll"]

FROM build AS publish
RUN dotnet publish "DeviceMicroservice.csproj" -c Release -o /device-microservice/publish

FROM base AS final
WORKDIR /device-microservice
COPY --from=publish /device-microservice/publish .
ENTRYPOINT ["dotnet", "DeviceMicroservice.dll"]