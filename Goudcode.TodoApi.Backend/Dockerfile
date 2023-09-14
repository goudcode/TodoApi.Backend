#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Goudcode.TodoApi.Backend/Goudcode.TodoApi.Backend.csproj", "Goudcode.TodoApi.Backend/"]
RUN dotnet restore "Goudcode.TodoApi.Backend/Goudcode.TodoApi.Backend.csproj"
COPY . .
WORKDIR "/src/Goudcode.TodoApi.Backend"
RUN dotnet build "Goudcode.TodoApi.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Goudcode.TodoApi.Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Goudcode.TodoApi.Backend.dll"]