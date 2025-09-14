FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/Web"
RUN dotnet publish "Web.csproj" --no-restore -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Web.dll"]