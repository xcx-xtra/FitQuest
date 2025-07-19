@echo off
echo Starting FitQuest Development Environment...
echo.

REM Check if .NET 9 SDK is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET 9 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo .NET SDK Version:
dotnet --version
echo.

REM Navigate to the solution directory
cd /d "%~dp0FitQuest"

REM Restore packages
echo Restoring NuGet packages...
dotnet restore FitQuest.sln
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo Building solution...
dotnet build FitQuest.sln --no-restore
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo Starting API and Client...
echo.
echo API will be available at: http://localhost:5124
echo Client will be available at: http://localhost:5174
echo.
echo Press Ctrl+C to stop both applications
echo.

REM Start API in background
start "FitQuest API" cmd /k "cd /d "%~dp0FitQuest\src\FitQuest.Api" && dotnet run"

REM Wait a moment for API to start
timeout /t 3 /nobreak >nul

REM Start Client in foreground
cd /d "%~dp0FitQuest\src\FitQuest.Client"
dotnet run