#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim-arm32v7 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TR064Exporter/TR064Exporter.csproj", "TR064Exporter/"]
COPY ["submodule/PS.FritzBox.API/PS.FritzBox.API/PS.FritzBox.API.csproj", "submodule/PS.FritzBox.API/PS.FritzBox.API/"]
RUN dotnet restore "TR064Exporter/TR064Exporter.csproj"
COPY . .
WORKDIR "/src/TR064Exporter"
RUN dotnet build "TR064Exporter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TR064Exporter.csproj"  -r linux-arm -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "TR064Exporter.dll"]