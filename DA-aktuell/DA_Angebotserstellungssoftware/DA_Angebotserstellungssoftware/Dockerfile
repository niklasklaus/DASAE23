FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DA_Angebotserstellungssoftware/DA_Angebotserstellungssoftware.csproj", "DA_Angebotserstellungssoftware/"]
RUN dotnet restore "DA_Angebotserstellungssoftware/DA_Angebotserstellungssoftware.csproj"
COPY . .
WORKDIR "/src/DA_Angebotserstellungssoftware"
RUN dotnet build "DA_Angebotserstellungssoftware.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DA_Angebotserstellungssoftware.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DA_Angebotserstellungssoftware.dll"]
