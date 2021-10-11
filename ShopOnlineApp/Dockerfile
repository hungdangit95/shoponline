#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ShopOnlineApp/ShopOnlineApp.csproj", "ShopOnlineApp/"]
COPY ["ShopOnlineApp.Data/ShopOnlineApp.Data.csproj", "ShopOnlineApp.Data/"]
COPY ["ShopOnlineApp.Infrastructure/ShopOnlineApp.Infrastructure.csproj", "ShopOnlineApp.Infrastructure/"]
COPY ["ShopOnlineApp.Utilities/ShopOnlineApp.Utilities.csproj", "ShopOnlineApp.Utilities/"]
COPY ["ShopOnlineApp.Application/ShopOnlineApp.Application.csproj", "ShopOnlineApp.Application/"]
COPY ["ShopOnlineApp.Data.EF/ShopOnlineApp.Data.EF.csproj", "ShopOnlineApp.Data.EF/"]
COPY ["ShopOnline.Application.Dapper/ShopOnline.Application.Dapper.csproj", "ShopOnline.Application.Dapper/"]
RUN dotnet restore "ShopOnlineApp/ShopOnlineApp.csproj"
COPY . .
WORKDIR "/src/ShopOnlineApp"
RUN dotnet build "ShopOnlineApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopOnlineApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopOnlineApp.dll"]