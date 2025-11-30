FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArlequimTech.Order.API/ArlequimTech.Order.API.csproj", "ArlequimTech.Order.API/"]
RUN dotnet restore "ArlequimTech.Order.API/ArlequimTech.Order.API.csproj"
COPY . .
WORKDIR "/src/ArlequimTech.Order.API"
RUN dotnet build "./ArlequimTech.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ArlequimTech.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArlequimTech.Order.API.dll"]
