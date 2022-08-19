#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["worker/Consumer_Worker.csproj", "worker/"]
RUN dotnet restore "worker/Consumer_Worker.csproj"
COPY . .
WORKDIR "/src/worker"
RUN dotnet build "Consumer_Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer_Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer_Worker.dll"]