FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cotacol.Website/Cotacol.Website.csproj", "Cotacol.Website/"]
RUN dotnet restore "Cotacol.Website/Cotacol.Website.csproj"
COPY . .
WORKDIR "/src/Cotacol.Website"
RUN dotnet build "Cotacol.Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cotacol.Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cotacol.Website.dll"]
