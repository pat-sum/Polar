#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["server/Server.Api.csproj", "Server.Api/"]
RUN dotnet restore "/src/server/Server.Api.csproj"
COPY . .
WORKDIR /src/Server.Api
RUN dotnet build "Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.Api.dll"]