FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /API

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /API
COPY --from=build-env /API/out .
ENTRYPOINT [ "dotnet", "DotNet.Docker.dll" ]