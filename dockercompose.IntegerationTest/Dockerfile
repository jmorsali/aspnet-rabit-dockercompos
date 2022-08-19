#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dockercompose.IntegerationTest/dockercompose.IntegerationTest.csproj", "dockercompose.IntegerationTest/"]
RUN dotnet restore "dockercompose.IntegerationTest/dockercompose.IntegerationTest.csproj"
COPY . .
WORKDIR "/src/dockercompose.IntegerationTest"
RUN dotnet build "dockercompose.IntegerationTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dockercompose.IntegerationTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dockercompose.IntegerationTest.dll"]