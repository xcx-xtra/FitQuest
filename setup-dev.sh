#!/bin/bash

echo "FitQuest Development Environment Setup"
echo "====================================="
echo

# Check if .NET 9 SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK is not installed or not in PATH"
    echo
    echo "Please install .NET 9 SDK from:"
    echo "https://dotnet.microsoft.com/download/dotnet/9.0"
    echo
    exit 1
fi

echo "✓ .NET SDK Version:"
dotnet --version
echo

# Navigate to the solution directory
cd "$(dirname "$0")/FitQuest"

# Clean previous builds
echo "Cleaning previous builds..."
dotnet clean FitQuest.sln > /dev/null 2>&1

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore FitQuest.sln
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages"
    exit 1
fi
echo "✓ Packages restored successfully"

# Build solution
echo
echo "Building solution..."
dotnet build FitQuest.sln --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi
echo "✓ Solution built successfully"

# Check if database exists
echo
echo "Checking database setup..."
if [ -f "src/FitQuest.Api/FitQuestDb.sqlite" ]; then
    echo "✓ SQLite database found"
else
    echo "ℹ SQLite database will be created on first API startup"
fi

echo
echo "====================================="
echo "Setup completed successfully!"
echo "====================================="
echo
echo "To start the development environment:"
echo "  - Run: ./start-dev.sh"
echo "  - Or manually:"
echo "    1. API: cd FitQuest/src/FitQuest.Api && dotnet run"
echo "    2. Client: cd FitQuest/src/FitQuest.Client && dotnet run"
echo
echo "URLs:"
echo "  - API: http://localhost:5124"
echo "  - Client: http://localhost:5174"
echo "  - API Documentation: http://localhost:5124/swagger"
echo