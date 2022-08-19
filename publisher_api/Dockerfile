#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["publisher_api/publisher_api.csproj", "publisher_api/"]
RUN dotnet restore "publisher_api/publisher_api.csproj"
COPY . .
WORKDIR "/src/publisher_api"
RUN dotnet build "publisher_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "publisher_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "publisher_api.dll"]