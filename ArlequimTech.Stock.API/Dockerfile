FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArlequimTech.Stock.API/ArlequimTech.Stock.API.csproj", "ArlequimTech.Stock.API/"]
RUN dotnet restore "ArlequimTech.Stock.API/ArlequimTech.Stock.API.csproj"
COPY . .
WORKDIR "/src/ArlequimTech.Stock.API"
RUN dotnet build "./ArlequimTech.Stock.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ArlequimTech.Stock.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArlequimTech.Stock.API.dll"]
