FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

# Build React
COPY client/package*.json client/
RUN cd client && npm install

COPY client/ client/
RUN cd client && npm run build

# Build .NET
COPY algorithm-visualizer.sln .
COPY AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj AlgorithmVisualizer.Api/
RUN dotnet restore AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj

COPY AlgorithmVisualizer.Api/ AlgorithmVisualizer.Api/

# Copy React output into wwwroot before publish
RUN mkdir -p AlgorithmVisualizer.Api/wwwroot && cp -r client/dist/. AlgorithmVisualizer.Api/wwwroot/

RUN dotnet publish AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AlgorithmVisualizer.Api.dll"]
