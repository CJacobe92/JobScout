#!/bin/sh

echo "[entrypoint] Running as UID: $(id -u)"

# Change ownership to the container user
chown $APP_UID:$APP_UID /https/aspnetapp.pfx

# Start the app
exec dotnet JobScout.API.dll
