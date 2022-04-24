#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["app/Organization/Organization.Api/Organization.Api.csproj", "app/Organization/Organization.Api/"]
RUN dotnet restore "app/Organization/Organization.Api/Organization.Api.csproj"
COPY . .
WORKDIR "/src/app/Organization/Organization.Api"
RUN dotnet build "Organization.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Organization.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Organization.Api.dll"]