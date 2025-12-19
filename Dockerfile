FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY BibliotecaApi.csproj ./
RUN dotnet restore

ENV ConnectionStrings__Database="Data Source=/data/Biblioteca.db"

COPY . .
WORKDIR /src/.
RUN dotnet publish -c Release -o /app/publish

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

RUN mkdir -p /data

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "BibliotecaApi.dll"]
