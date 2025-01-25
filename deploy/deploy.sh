#!/bin/bash

# Navigate to the app directory
cd /home/ruslany/src/sample-app/app || exit

# Build the webapp
echo "Building the webapp..."
dotnet restore
dotnet build --configuration Release

# Package the webapp
echo "Packaging the webapp..."
dotnet publish --configuration Release --output ../deploy/publish

echo "Build and packaging completed successfully."