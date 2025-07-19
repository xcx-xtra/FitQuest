@echo off
echo FitQuest Development Environment Setup
echo =====================================
echo.

REM Check if .NET 9 SDK is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo.
    echo Please install .NET 9 SDK from:
    echo https://dotnet.microsoft.com/download/dotnet/9.0
    echo.
    pause
    exit /b 1
)

echo ✓ .NET SDK Version: 
dotnet --version
echo.

REM Navigate to the solution directory
cd /d "%~dp0FitQuest"

REM Clean previous builds
echo Cleaning previous builds...
dotnet clean FitQuest.sln >nul 2>&1

REM Restore packages
echo Restoring NuGet packages...
dotnet restore FitQuest.sln
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo ✓ Packages restored successfully

REM Build solution
echo.
echo Building solution...
dotnet build FitQuest.sln --no-restore
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)
echo ✓ Solution built successfully

REM Check if database exists, if not it will be created on first run
echo.
echo Checking database setup...
if exist "src\FitQuest.Api\FitQuestDb.sqlite" (
    echo ✓ SQLite database found
) else (
    echo ℹ SQLite database will be created on first API startup
)

echo.
echo =====================================
echo Setup completed successfully!
echo =====================================
echo.
echo To start the development environment:
echo   - Run: start-dev.bat
echo   - Or manually:
echo     1. API: cd FitQuest\src\FitQuest.Api && dotnet run
echo     2. Client: cd FitQuest\src\FitQuest.Client && dotnet run
echo.
echo URLs:
echo   - API: http://localhost:5124
echo   - Client: http://localhost:5174
echo   - API Documentation: http://localhost:5124/swagger
echo.
pause