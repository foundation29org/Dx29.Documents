#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Dx29.Documents.WebAPI/Dx29.Documents.WebAPI.csproj", "Dx29.Documents.WebAPI/"]
RUN dotnet restore "Dx29.Documents.WebAPI/Dx29.Documents.WebAPI.csproj"
COPY . .
WORKDIR "/src/Dx29.Documents.WebAPI"
RUN dotnet build "Dx29.Documents.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dx29.Documents.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dx29.Documents.WebAPI.dll"]