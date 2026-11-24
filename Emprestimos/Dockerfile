FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN PROJECT_FILE="$(find . -name '*.csproj' -not -path '*/bin/*' -not -path '*/obj/*' | head -n 1)" && \
    test -n "$PROJECT_FILE" && \
    dotnet restore "$PROJECT_FILE" && \
    dotnet publish "$PROJECT_FILE" \
        --configuration Release \
        --output /app/publish \
        --no-restore \
        /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_CONTENTROOT=/app
EXPOSE 8080
VOLUME ["/data"]

RUN mkdir -p /data && chown app:app /app /data
COPY --from=build --chown=app:app /app/publish .

USER app
WORKDIR /data
ENTRYPOINT ["sh", "-c", "RUNTIMECONFIG=$(find /app -maxdepth 1 -name '*.runtimeconfig.json' | head -n 1); test -n \"$RUNTIMECONFIG\"; exec dotnet \"${RUNTIMECONFIG%.runtimeconfig.json}.dll\""]
