FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Web.Common/Web.Common.csproj", "Web.Common/"]
COPY ["WebUtilities/WebUtilities.csproj", "WebUtilities/"]
COPY ["Web.Common.Data/Web.Common.Data.csproj", "Web.Common.Data/"]
COPY ["Web.Common.Entity/Web.Common.Entity.csproj", "Web.Common.Entity/"]
RUN dotnet restore "Web.Common/Web.Common.csproj"
COPY . .
WORKDIR "/src/Web.Common"
RUN dotnet build "Web.Common.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Common.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Common.dll"]
