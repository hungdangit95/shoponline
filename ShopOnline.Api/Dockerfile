#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ShopOnline.Api/ShopOnline.Api.csproj", "ShopOnline.Api/"]
COPY ["../ShopOnline.Share/ShopOnline.Share.csproj", "../ShopOnline.Share/"]
COPY ["ShopOnlineApp.Data/ShopOnlineApp.Data.csproj", "ShopOnlineApp.Data/"]
COPY ["ShopOnlineApp.Infrastructure/ShopOnlineApp.Infrastructure.csproj", "ShopOnlineApp.Infrastructure/"]
COPY ["ShopOnlineApp.Utilities/ShopOnlineApp.Utilities.csproj", "ShopOnlineApp.Utilities/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["ShopOnlineApp.Application/ShopOnlineApp.Application.csproj", "ShopOnlineApp.Application/"]
COPY ["ShopOnlineApp.Data.EF/ShopOnlineApp.Data.EF.csproj", "ShopOnlineApp.Data.EF/"]
COPY ["ShopOnline.Application.Dapper/ShopOnline.Application.Dapper.csproj", "ShopOnline.Application.Dapper/"]
COPY ["../DataProvider/DataProvider.csproj", "../DataProvider/"]
COPY ["ShopOnline.Service.DataModel/ShopOnline.Service.DataModel.csproj", "ShopOnline.Service.DataModel/"]
RUN dotnet restore "ShopOnline.Api/ShopOnline.Api.csproj"
COPY . .
WORKDIR "/src/ShopOnline.Api"
RUN dotnet build "ShopOnline.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopOnline.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopOnline.Api.dll"]