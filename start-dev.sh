#!/bin/bash

echo "Starting FitQuest Development Environment..."
echo

# Check if .NET 9 SDK is installed
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK is not installed or not in PATH"
    echo "Please install .NET 9 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi

echo ".NET SDK Version:"
dotnet --version
echo

# Navigate to the solution directory
cd "$(dirname "$0")/FitQuest"

# Restore packages
echo "Restoring NuGet packages..."
dotnet restore FitQuest.sln
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages"
    exit 1
fi

echo
echo "Building solution..."
dotnet build FitQuest.sln --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi

echo
echo "Starting API and Client..."
echo
echo "API will be available at: http://localhost:5124"
echo "Client will be available at: http://localhost:5174"
echo
echo "Press Ctrl+C to stop both applications"
echo

# Function to cleanup background processes
cleanup() {
    echo
    echo "Stopping applications..."
    kill $API_PID 2>/dev/null
    exit 0
}

# Set trap to cleanup on script exit
trap cleanup SIGINT SIGTERM

# Start API in background
cd "src/FitQuest.Api"
dotnet run &
API_PID=$!

# Wait a moment for API to start
sleep 3

# Start Client in foreground
cd "../FitQuest.Client"
dotnet run

# Cleanup when client exits
cleanup