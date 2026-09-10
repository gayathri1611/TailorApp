# Stage 1: Build Angular frontend
FROM node:20-alpine AS node_build

WORKDIR /app/frontend

COPY Frontend/tailor-app-ui/package*.json ./

RUN npm ci

COPY Frontend/tailor-app-ui/ .

RUN npm run build


# Stage 2: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

# Remove existing wwwroot
RUN rm -rf Backend/TailorApp.API/wwwroot

# Copy Angular build output into ASP.NET Core wwwroot
COPY --from=node_build /app/frontend/dist/tailor-app-ui/browser Backend/TailorApp.API/wwwroot

# Restore .NET dependencies
RUN dotnet restore "Backend/TailorApp.API/TailorApp.API.csproj"

# Publish .NET application
RUN dotnet publish "Backend/TailorApp.API/TailorApp.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "TailorApp.API.dll"]