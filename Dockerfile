#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12 AS base
RUN apk add --update --no-cache nodejs && node --version
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.12 AS build
RUN apk add --update --no-cache nodejs nodejs-npm && node --version && npm --version
WORKDIR /src
COPY ["VueTemplate/VueTemplate.csproj", "VueTemplate/"]
RUN dotnet restore "VueTemplate/VueTemplate.csproj"
COPY . .
WORKDIR "/src/VueTemplate"
RUN dotnet build "VueTemplate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VueTemplate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VueTemplate.dll"]