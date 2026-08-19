FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "ServiceHost/ServiceHost.csproj"

COPY . .
WORKDIR "/src/ServiceHost"
RUN dotnet publish "ServiceHost.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ServiceHost.dll"]
