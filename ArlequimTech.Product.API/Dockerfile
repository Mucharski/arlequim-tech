FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8085
EXPOSE 8086

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArlequimTech.Product.API/ArlequimTech.Product.API.csproj", "ArlequimTech.Product.API/"]
RUN dotnet restore "ArlequimTech.Product.API/ArlequimTech.Product.API.csproj"
COPY . .
WORKDIR "/src/ArlequimTech.Product.API"
RUN dotnet build "./ArlequimTech.Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ArlequimTech.Product.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArlequimTech.Product.API.dll"]
