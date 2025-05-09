# 🏆 FitQuest

A gamified fitness challenge tracker built with Blazor WebAssembly and ASP.NET Core. Users can set daily goals, earn points, unlock badges, and compete on real-time leaderboards.

---

## 🚀 Technologies Used

### Frontend
- **Blazor WebAssembly**
- Authentication with JWT tokens
- Component-based UI (Goals Editor, Dashboard, Leaderboard)

### Backend
- **ASP.NET Core Web API**
- **Entity Framework Core** with SQL Server
- **ASP.NET Identity** for user management
- **SignalR** for real-time leaderboard updates

### DevOps & Deployment
- GitHub Actions (CI/CD)
- Azure App Service (future deployment)
- Logging: Serilog + Azure Application Insights (planned)

---

## 📦 Features

- ✅ User registration and JWT login
- ✅ Set daily fitness goals
- ✅ Earn points and view point history
- ✅ Dashboard showing total points and event log
- ✅ Real-time leaderboard using SignalR (coming)
- ✅ Admin panel to manage badges & challenges (coming)
- 🛠️ Azure deployment setup (coming)

---

## 🧱 Database Schema (EF Core)

| Entity       | Description                                      |
|--------------|--------------------------------------------------|
| `AppUser`    | Identity user with unique ID                    |
| `DailyGoal`  | User-set goals with completion status           |
| `PointEvent` | Earned points tied to goals and events          |
| `Badge`      | Milestone rewards unlocked by users             |
| `Challenge`  | Group-based competitive challenges              |

---

## 🔐 Authentication

- ASP.NET Core Identity + JWT bearer tokens
- Stored in browser local storage for Blazor WebAssembly
- Authorized endpoints for user-specific goal/point actions

---

## 📡 Current API Endpoints

| Method | Endpoint                          | Description                       |
|--------|-----------------------------------|-----------------------------------|
| `POST` | `/api/goals`                      | Create or update daily goals      |
| `GET`  | `/api/users/{userId}/points`      | Fetch user point summary & log    |

---

## 📋 Blazor Pages Implemented

- `Register.razor` – Creates user account
- `Login.razor` – Logs in and stores JWT token
- `Dashboard.razor` – Shows point totals and goal summaries
- `Goals.razor` – (WIP) Create/update daily fitness goals

---

## 🧪 Local Development Setup

1. **Clone the repo**

```bash
git clone https://github.com/your-username/fitquest.git
cd fitquest
Update your DB

bash
Copy code
dotnet ef database update
Run the app

bash
Copy code
dotnet run
Launch in browser

Backend: https://localhost:5001

Frontend: https://localhost:5002 (Blazor WASM)

✅ Testing with Postman
Register

arduino
Copy code
POST /api/account/register
Login

bash
Copy code
POST /api/account/login
Get Points

bash
Copy code
GET /api/users/{userId}/points
Headers: Authorization: Bearer {your_token}

📅 Project Sprint Timeline
Sprint	Date Range	Key Goals	Status
Sprint 1	Apr 26–28	Git, EF Core setup	✅ Done
Sprint 2	Apr 29–May 1	JWT Auth, Register/Login UI	✅ Done
Sprint 3	May 2–4	Daily Goal backend & UI	✅ Done
Sprint 4	May 5–7	Points API & Dashboard integration	✅ Done
Sprint 7	May 14–16	SignalR Leaderboard	🔜 Next
Sprint 8	May 17–19	Azure Deployment & Admin Panel	🔜 Upcoming

👏 Contributing
Want to contribute? Create a pull request or fork the repo to propose new features, bug fixes, or styling improvements.

📜 License
MIT License. See LICENSE file for more info.

🌟 Future Features
🥇 Badge system for milestones

🧠 AI-generated daily challenge suggestions

📱 Integration with wearable devices (Fitbit, Apple Health)

🌐 Social sharing for goals and achievements
