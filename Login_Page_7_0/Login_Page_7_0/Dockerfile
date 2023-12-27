FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Login_Page_7_0/Login_Page_7_0.csproj", "Login_Page_7_0/"]
RUN dotnet restore "Login_Page_7_0/Login_Page_7_0.csproj"
COPY . .
WORKDIR "/src/Login_Page_7_0"
RUN dotnet build "Login_Page_7_0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Login_Page_7_0.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Login_Page_7_0.dll"]
