# Utilizar la imagen base de .NET Core SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo dentro del contenedor
WORKDIR /source

# Copiar los archivos del proyecto al contenedor
COPY . .
# Restaurar las dependencias y compilar la aplicación
RUN dotnet restore "src/SmartHome.WebApi/SmartHome.WebApi.csproj" --disable-parallel
RUN dotnet publish "src/SmartHome.WebApi/SmartHome.WebApi.csproj" -c release -o /app --no-restore


# Etapa de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copiar los archivos publicados de la etapa anterior
COPY --from=build /app ./

EXPOSE 5222

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "SmartHome.WebApi.dll"]