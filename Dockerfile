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
COPY api/api.csproj api/
RUN dotnet restore api/api.csproj

COPY api/ api/

# Copy React output into wwwroot before publish
RUN mkdir -p api/wwwroot && \
    cp -r client/dist/* api/wwwroot/ && \
    ls -la api/wwwroot/

RUN dotnet publish api/api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "api.dll"]
