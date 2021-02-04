#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
EXPOSE 6000
WORKDIR /analytics-microservice

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /analytics-microservice/src
COPY . .
RUN dotnet restore "AnalyticsMicroservice.csproj"
RUN dotnet build "AnalyticsMicroservice.csproj" -c Release -o /analytics-microservice/build
ENTRYPOINT ["dotnet", "AnalyticsMicroservice.dll"]

FROM build AS publish
RUN dotnet publish "AnalyticsMicroservice.csproj" -c Release -o /analytics-microservice/publish

FROM base AS final
WORKDIR /analytics-microservice
COPY --from=publish /analytics-microservice/publish .
ENTRYPOINT ["dotnet", "AnalyticsMicroservice.dll"]