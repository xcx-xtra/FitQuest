FitQuest Project

Overview

FitQuest is a gamified fitness tracker built using Blazor WebAssembly for the frontend, ASP.NET Core Web API for the backend, and SignalR for real-time communication. It helps users set daily fitness goals, earn points, and track progress on a live leaderboard.

Core Technologies

Frontend: Blazor WebAssembly

Backend: ASP.NET Core Web API with Entity Framework Core (Code-First)

Database: SQL Server

Authentication: ASP.NET Core Identity with JWT & Cookie auth

Real-Time: SignalR for live leaderboard updates

DevOps: GitHub Actions, Azure App Service (No Docker)

Project Milestones

✅ Sprint 1 (Apr 26–28, 2025)

Git repo and solution structure created

EF Core models (User, DailyGoal, Badge) defined

SQL Server integration

✅ Sprint 2 (Apr 29–May 1, 2025)

ASP.NET Core Identity integration

JWT authentication implemented

/register and /login endpoints functional

✅ Sprint 3 (May 2–4, 2025) — In Progress



🔜 Sprint 4 (May 5–7, 2025)

SignalR Hub for real-time leaderboard

Broadcast points and rank changes to clients

📅 Sprint 7 (May 14–16, 2025)

Finalize Blazor UI and live updates

🚀 Sprint 8 (May 17–19, 2025)

Admin Panel, deployment to Azure App Service

API Endpoints

GoalsController

POST /api/goals – Create a new daily goal

GET /api/goals/user/{userId} – Fetch user’s daily goals

PointsController

POST /api/points – Award points to user

GET /api/points/user/{userId} – Get user’s point history

Upcoming Features

SignalR Live Leaderboard

Admin Panel to manage users and badges

Azure Notifications and Reminders

Integration with wearable fitness devices

How to Run Locally

# Clone the repo
git clone https://github.com/xcx-xtra/FitQuest.git
cd FitQuest

# Restore packages & run the API
cd src/FitQuest.Api
dotnet restore
 dotnet run

# Run the Blazor client
cd ../FitQuest.Client
dotnet run

Deployment

GitHub Actions will build and deploy to Azure App Service.

Slot-based deployments used for zero downtime.

Logs and telemetry handled via Azure Application Insights.

Security

HTTPS and HSTS enforced

CSRF and secure headers middleware in place

Serilog logging integrated

Contributors

Webster Boeing — Developer / Project Lead

License

MIT License

