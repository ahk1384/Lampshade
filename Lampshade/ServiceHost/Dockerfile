FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServiceHost/ServiceHost.csproj", "ServiceHost/"]
COPY ["AccountManagement.Infrastructure.Configuration/AccountManagement.Infrastructure.Configuration.csproj", "AccountManagement.Infrastructure.Configuration/"]
COPY ["AccountManagement.Application/AccountManagement.Application.csproj", "AccountManagement.Application/"]
COPY ["AccountManagement.Application.Contract/AccountManagement.Application.Contract.csproj", "AccountManagement.Application.Contract/"]
COPY ["0_Framework/0_Framework.csproj", "0_Framework/"]
COPY ["AccountManagement.Domain/AccountManagement.Domain.csproj", "AccountManagement.Domain/"]
COPY ["AccountManagement.Infrastructure.EFCore/AccountManagement.Infrastructure.EFCore.csproj", "AccountManagement.Infrastructure.EFCore/"]
COPY ["BlogManagemenet.Infrastructure.Configuration/BlogManagement.Infrastructure.Configuration.csproj", "BlogManagemenet.Infrastructure.Configuration/"]
COPY ["BlogManagement,Application.Contract/BlogManagement.Application.Contract.csproj", "BlogManagement,Application.Contract/"]
COPY ["BlogManagement.Application/BlogManagement.Application.csproj", "BlogManagement.Application/"]
COPY ["BlogManagmenet.Domain/BlogManagement.Domain.csproj", "BlogManagmenet.Domain/"]
COPY ["BlogManagement.Infrastructure/BlogManagement.Infrastructure.EFCore.csproj", "BlogManagement.Infrastructure/"]
COPY ["_01_LampshadeQuery/_01_LampshadeQuery.csproj", "_01_LampshadeQuery/"]
COPY ["CommentManagement.Domain/CommentManagement.Domain.csproj", "CommentManagement.Domain/"]
COPY ["CommnetManagement.Application.Contracts/CommentManagement.Application.Contracts.csproj", "CommnetManagement.Application.Contracts/"]
COPY ["CommentManagement.Infrastructure.EFCore/CommentManagement.Infrastructure.EFCore.csproj", "CommentManagement.Infrastructure.EFCore/"]
COPY ["DiscountManagmenet.Infrastructure.EFCore/DiscountManagement.Infrastructure.EFCore.csproj", "DiscountManagmenet.Infrastructure.EFCore/"]
COPY ["DiscountManagmenet.Application.Contracts/DiscountManagement.Application.Contracts.csproj", "DiscountManagmenet.Application.Contracts/"]
COPY ["ShopManagemnt.Applocation.Contracts/ShopManagement.Application.Contracts.csproj", "ShopManagemnt.Applocation.Contracts/"]
COPY ["DiscountManagment.Domain/DiscountManagement.Domain.csproj", "DiscountManagment.Domain/"]
COPY ["ShopManagementDomain/ShopManagementDomain.csproj", "ShopManagementDomain/"]
COPY ["SM.Infrastructure.EFCore/SM.Infrastructure.EFCore.csproj", "SM.Infrastructure.EFCore/"]
COPY ["InventoryManagement.Domain/InventoryManagement.Domain.csproj", "InventoryManagement.Domain/"]
COPY ["InventoryManagement.Application.Contracts/InventoryManagement.Application.Contracts.csproj", "InventoryManagement.Application.Contracts/"]
COPY ["InventoryManagement.Infrastructure.EFCore/InventoryManagement.Infrastructure.EFCore.csproj", "InventoryManagement.Infrastructure.EFCore/"]
COPY ["CommentManagement.Infrastructure.Configuration/CommentManagement.Infrastructure.Configuration.csproj", "CommentManagement.Infrastructure.Configuration/"]
COPY ["CommnetManagement.Application/CommentManagement.Application.csproj", "CommnetManagement.Application/"]
COPY ["DiscountManagmenet.Infrastructure.Configuration/DiscountManagement.Infrastructure.Configuration.csproj", "DiscountManagmenet.Infrastructure.Configuration/"]
COPY ["DiscountManagmenet.Application/DiscountManagement.Application.csproj", "DiscountManagmenet.Application/"]
COPY ["InventoryManagement.Infrastructure.Configuration/InventoryManagement.Infrastructure.Configuration.csproj", "InventoryManagement.Infrastructure.Configuration/"]
COPY ["InventoryManagement.Application/InventoryManagement.Application.csproj", "InventoryManagement.Application/"]
COPY ["ShopManagment.Infrastructure.Configuration/ShopManagement.Infrastructure.Configuration.csproj", "ShopManagment.Infrastructure.Configuration/"]
COPY ["ShopManagement.Application/ShopManagement.Application.csproj", "ShopManagement.Application/"]
COPY ["ShopManagement.Infrastructure.AccountAcl/ShopManagement.Infrastructure.AccountAcl.csproj", "ShopManagement.Infrastructure.AccountAcl/"]
COPY ["ShopManagement.Infrastructure.InventoryAcl/ShopManagement.Infrastructure.InventoryAcl.csproj", "ShopManagement.Infrastructure.InventoryAcl/"]
RUN dotnet restore "ServiceHost/ServiceHost.csproj"
COPY . .
WORKDIR "/src/ServiceHost"
RUN dotnet build "./ServiceHost.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ServiceHost.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ServiceHost.dll"]
