FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /API
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /API

COPY ["API/API.csproj", "/API/"]
RUN dotnet restore "API/API.csproj"

COPY . .
WORKDIR /API/API
RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /API
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "API.dll" ]