FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PDFGenerierung_iText/PDFGenerierung_iText.csproj", "PDFGenerierung_iText/"]
RUN dotnet restore "PDFGenerierung_iText/PDFGenerierung_iText.csproj"
COPY . .
WORKDIR "/src/PDFGenerierung_iText"
RUN dotnet build "PDFGenerierung_iText.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PDFGenerierung_iText.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PDFGenerierung_iText.dll"]
