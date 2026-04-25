FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

COPY algorithm-visualizer.sln .
COPY AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj AlgorithmVisualizer.Api/
RUN dotnet restore

COPY . .
RUN dotnet publish AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AlgorithmVisualizer.Api.dll"]
