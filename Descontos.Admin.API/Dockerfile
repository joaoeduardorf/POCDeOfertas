#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Descontos.Admin.API/Descontos.Admin.API.csproj", "Descontos.Admin.API/"]
RUN dotnet restore "Descontos.Admin.API/Descontos.Admin.API.csproj"
COPY . .
WORKDIR "/src/Descontos.Admin.API"
RUN dotnet build "Descontos.Admin.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Descontos.Admin.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Descontos.Admin.API.dll"]