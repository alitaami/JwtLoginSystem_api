# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

# Use the SDK image to build the app
COPY JwtLoginSystem_api/JwtLoginSystem_api.csproj JwtLoginSystem_api/
COPY WebFramework/WebFramework.csproj WebFramework/
COPY Services/Services.csproj Services/
COPY Data/Data.csproj Data/              # <-- Add this line
COPY Entity/Entity.csproj Entity/        # <-- Add this line
COPY Common/Common.csproj Common/        # <-- Add this line

# Restore all projects
RUN dotnet restore "JwtLoginSystem_api/JwtLoginSystem_api.csproj"
RUN dotnet restore "WebFramework/WebFramework.csproj"
RUN dotnet restore "Services/Services.csproj"
RUN dotnet restore "Data/Data.csproj"      # <-- Add this line
RUN dotnet restore "Entity/Entity.csproj"  # <-- Add this line
RUN dotnet restore "Common/Common.csproj"  # <-- Add this line

# Copy everything else and build
COPY . .
WORKDIR "/src/JwtLoginSystem_api"
RUN dotnet build "JwtLoginSystem_api.csproj" -c Release -o /app/build

FROM m
