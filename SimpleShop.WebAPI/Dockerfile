FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleShop.WebAPI/SimpleShop.WebAPI.csproj", "SimpleShop.WebAPI/"]
COPY ["SimpleShop.Data/SimpleShop.Data.csproj", "SimpleShop.Data/"]
COPY ["SimpleShop.Models/SimpleShop.Models.csproj", "SimpleShop.Models/"]
COPY ["SimpleShop.Services/SimpleShop.Services.csproj", "SimpleShop.Services/"]
COPY ["SimpleShop.Cache/SimpleShop.Cache.csproj", "SimpleShop.Cache/"]
RUN dotnet restore "SimpleShop.WebAPI/SimpleShop.WebAPI.csproj"
COPY . .
WORKDIR "/src/SimpleShop.WebAPI"
RUN dotnet build "SimpleShop.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleShop.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleShop.WebAPI.dll"]
