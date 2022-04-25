FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://*:80

# Copy everything and build
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["NinjaHeaven/NinjaHeaven.csproj", "NinjaHeaven/"]
RUN dotnet restore "NinjaHeaven/NinjaHeaven.csproj"
COPY . .
WORKDIR "/src/NinjaHeaven"
RUN dotnet build "NinjaHeaven.csproj" -c Release -o /app/build

# Build and publish a release
FROM build AS publish
RUN dotnet publish "NinjaHeaven.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NinjaHeaven.dll"]
