FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8095
EXPOSE 8096

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArlequimTech.Order.API/ArlequimTech.Order.API.csproj", "ArlequimTech.Order.API/"]
COPY ["ArlequimTech.Order.Application/ArlequimTech.Order.Application.csproj", "ArlequimTech.Order.Application/"]
COPY ["ArlequimTech.Order.Domain/ArlequimTech.Order.Domain.csproj", "ArlequimTech.Order.Domain/"]
COPY ["ArlequimTech.Order.Infrastructure/ArlequimTech.Order.Infrastructure.csproj", "ArlequimTech.Order.Infrastructure/"]
COPY ["ArlequimTech.Core/ArlequimTech.Core.csproj", "ArlequimTech.Core/"]
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
