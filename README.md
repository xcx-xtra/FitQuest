# FitQuest

A fitness tracking and gamification application that combines workout management with quest-like progression mechanics. Built with .NET 9, Blazor WebAssembly, and ASP.NET Core Web API.

## 🚀 Quick Start

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (required)
- Git (for cloning the repository)

**That's it!** No Docker, SQL Server, or additional dependencies required.

### Setup & Run

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd FitQuest
   ```

2. **Run setup script** (recommended for first-time setup)

   ```bash
   # Windows
   setup-dev.bat

   # Linux/macOS
   ./setup-dev.sh
   ```

3. **Start the application**

   ```bash
   # Windows
   start-dev.bat

   # Linux/macOS
   ./start-dev.sh
   ```

4. **Access the application**
   - **Client**: http://localhost:5174
   - **API**: http://localhost:5124
   - **API Documentation**: http://localhost:5124/swagger

The application will automatically:

- Create the SQLite database on first run
- Run Entity Framework migrations
- Seed initial data if needed
- Configure CORS for local development

## 🏗️ Architecture

FitQuest follows a clean architecture pattern with three main projects:

```
FitQuest/
├── FitQuest/                   # Main solution directory
│   ├── FitQuest.sln           # Visual Studio solution file
│   └── src/
│       ├── FitQuest.Api/      # ASP.NET Core Web API
│       ├── FitQuest.Client/   # Blazor WebAssembly
│       └── FitQuest.Shared/   # Shared models and utilities
├── setup-dev.bat/.sh          # Development setup scripts
├── start-dev.bat/.sh          # Development startup scripts
└── README.md
```

### Project Responsibilities

- **FitQuest.Api**: Backend API, authentication, database operations, SignalR hubs, automatic migrations
- **FitQuest.Client**: Frontend UI, client-side routing, API consumption, error handling
- **FitQuest.Shared**: Common models, DTOs, utilities shared between API and Client

### Key Features

- **Zero-Configuration Development**: No Docker or SQL Server setup required
- **Automatic Database Management**: SQLite database created and migrated automatically
- **Real-time Communication**: SignalR for live updates and notifications
- **Modern CSS Design System**: CSS variables-based styling without external frameworks
- **Comprehensive Error Handling**: User-friendly error messages and logging
- **Hot Reload Support**: Both API and Client support development hot reload

## 🛠️ Technology Stack

### Core Technologies

- **.NET 9**: Latest .NET framework for all projects
- **C#**: Primary programming language with nullable reference types enabled
- **Blazor WebAssembly**: Interactive client-side web framework
- **ASP.NET Core Web API**: Backend API framework
- **Entity Framework Core**: ORM for database operations
- **SQLite**: Primary database for development (zero-config)
- **SignalR**: Real-time communication between client and server

### Key Dependencies

- **JWT Authentication**: Secure user authentication with ASP.NET Core Identity
- **CSS Variables**: Modern design system without external CSS frameworks
- **Structured Logging**: Built-in logging with development-friendly configuration
- **Error Boundaries**: Blazor error boundaries for component isolation
- **Hot Reload**: Development-time hot reload for both API and Client

## 💾 Database

The application uses **SQLite** for development, providing a zero-configuration database solution:

### Features

- **Zero Configuration**: No database server installation required
- **File-based Storage**: Database stored as `FitQuestDb.sqlite` in the API project
- **Automatic Creation**: Database and tables created automatically on first run
- **Auto-Migration**: Entity Framework migrations run automatically in development
- **Easy Backup**: Simply copy the `.sqlite` file to backup your data
- **Cross-Platform**: Works identically on Windows, macOS, and Linux

### Database Location

```
FitQuest/src/FitQuest.Api/FitQuestDb.sqlite
```

### Migration Management

- **Development**: Migrations run automatically on startup
- **New Migrations**: Use `dotnet ef migrations add <MigrationName>` in the API project
- **Reset Database**: Delete the `.sqlite` file and restart the API to recreate

The database includes tables for users, goals, progress tracking, and all fitness-related data with proper relationships and constraints.

## 🎨 CSS Design System

FitQuest uses a modern CSS variable-based design system that replaces traditional CSS frameworks like Bootstrap and Tailwind. This approach provides better maintainability, theming capabilities, and performance.

### Design Philosophy

- **CSS Variables**: All design tokens defined as CSS custom properties
- **Component-Based**: Semantic component classes instead of utility classes
- **Modern CSS**: Uses modern CSS features like Grid, Flexbox, and custom properties
- **No External Dependencies**: Zero CSS framework dependencies
- **Responsive**: Mobile-first responsive design built-in

### Design Tokens

The design system includes comprehensive design tokens:

```css
:root {
  /* Color Palette */
  --color-primary: #1b6ec2;
  --color-primary-hover: #1861ac;
  --color-success: #26b050;
  --color-danger: #dc3545;
  --color-warning: #ffc107;
  --color-info: #17a2b8;

  /* Neutral Colors */
  --color-white: #ffffff;
  --color-light: #f8f9fa;
  --color-gray-100: #f1f3f4;
  --color-gray-500: #adb5bd;
  --color-gray-900: #212529;
  --color-dark: #343a40;

  /* Typography */
  --font-family-primary: "Inter", -apple-system, BlinkMacSystemFont, "Segoe UI",
    sans-serif;
  --font-size-xs: 0.75rem;
  --font-size-sm: 0.875rem;
  --font-size-base: 1rem;
  --font-size-lg: 1.125rem;
  --font-size-xl: 1.25rem;
  --font-size-2xl: 1.5rem;

  /* Spacing Scale */
  --spacing-xs: 0.25rem;
  --spacing-sm: 0.5rem;
  --spacing-md: 1rem;
  --spacing-lg: 1.5rem;
  --spacing-xl: 2rem;
  --spacing-2xl: 3rem;

  /* Border Radius */
  --radius-sm: 0.25rem;
  --radius-md: 0.375rem;
  --radius-lg: 0.5rem;
  --radius-xl: 0.75rem;

  /* Shadows */
  --shadow-sm: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  --shadow-md: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
  --shadow-lg: 0 10px 15px -3px rgba(0, 0, 0, 0.1);

  /* Transitions */
  --transition-fast: 150ms ease-in-out;
  --transition-normal: 250ms ease-in-out;
}
```

### Component Classes

#### Buttons

```css
.btn                    /* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
/* Base button styles */
.btn-primary           /* Primary action button */
.btn-secondary         /* Secondary action button */
.btn-success           /* Success/positive action */
.btn-danger            /* Destructive action */
.btn-outline-primary   /* Outlined primary button */
.btn-link              /* Link-styled button */
.btn-sm, .btn-lg; /* Size variants */
```

#### Forms

```css
.form-group            /* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
/* Form field container */
.form-label            /* Form field labels */
.form-control          /* Input, textarea, select */
.form-control-sm       /* Small form controls */
.form-control-lg       /* Large form controls */
.form-check            /* Checkbox/radio container */
.form-check-input      /* Checkbox/radio input */
.form-check-label      /* Checkbox/radio label */
.form-floating; /* Floating label forms */
```

#### Cards

```css
.card                  /* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
/* Card container */
.card-header           /* Card header section */
.card-body             /* Card main content */
.card-footer           /* Card footer section */
.card-title            /* Card title */
.card-subtitle         /* Card subtitle */
.card-primary          /* Primary themed card */
.card-success; /* Success themed card */
```

#### Layout

```css
.row                   /* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
/* Flex row container */
.col                   /* Flexible column */
.col-1 to .col-12      /* Fixed width columns */
.col-md-1 to .col-md-12; /* Responsive columns */
```

### Usage Examples

#### Basic Card with Button

```html
<div class="card">
  <div class="card-header">
    <h3 class="card-title">Goal Progress</h3>
  </div>
  <div class="card-body">
    <p class="card-text">Track your fitness journey</p>
    <button class="btn btn-primary">Update Goal</button>
  </div>
</div>
```

#### Form with Validation

```html
<div class="form-group">
  <label class="form-label">Goal Name</label>
  <input type="text" class="form-control" placeholder="Enter goal name" />
  <div class="validation-message">This field is required</div>
</div>
```

#### Responsive Grid

```html
<div class="row">
  <div class="col-12 col-md-6">
    <div class="card">...</div>
  </div>
  <div class="col-12 col-md-6">
    <div class="card">...</div>
  </div>
</div>
```

### Customization

To customize the design system, modify the CSS variables in `FitQuest.Client/wwwroot/css/app.css`:

```css
:root {
  /* Override default colors */
  --color-primary: #your-brand-color;
  --color-success: #your-success-color;

  /* Adjust spacing scale */
  --spacing-md: 1.25rem;

  /* Change typography */
  --font-family-primary: "Your Font", sans-serif;
}
```

### Benefits

- **Performance**: No external CSS framework to download
- **Maintainability**: Centralized design tokens
- **Consistency**: Systematic approach to spacing, colors, and typography
- **Theming**: Easy to create themes by changing CSS variables
- **Modern**: Uses latest CSS features and best practices

## 🔧 Development

### Manual Commands

If you prefer to run commands manually instead of using the scripts:

```bash
# Navigate to solution directory
cd FitQuest

# Restore packages
dotnet restore FitQuest.sln

# Build solution
dotnet build FitQuest.sln

# Run API (Terminal 1)
cd src/FitQuest.Api
dotnet run

# Run Client (Terminal 2 - in a new terminal)
cd src/FitQuest.Client
dotnet run
```

### Database Migrations

Entity Framework migrations are handled automatically in development mode. The API will:

- Create the database if it doesn't exist
- Run pending migrations on startup
- Seed initial data if needed

#### Manual Migration Commands

If you need to create new migrations:

```bash
# Navigate to API project
cd FitQuest/src/FitQuest.Api

# Add a new migration
dotnet ef migrations add YourMigrationName

# Apply migrations manually (optional - happens automatically)
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

### Project Structure

```
FitQuest/
├── FitQuest.sln              # Visual Studio solution
└── src/
    ├── FitQuest.Api/         # ASP.NET Core Web API
    │   ├── Controllers/      # API endpoints and controllers
    │   ├── Data/            # Database context and configurations
    │   ├── Models/          # Entity models
    │   ├── Services/        # Business logic services
    │   ├── Middleware/      # Custom middleware (error handling, etc.)
    │   ├── Configuration/   # Application configuration
    │   ├── Migrations/      # Entity Framework migrations
    │   ├── Program.cs       # Application startup and configuration
    │   ├── appsettings.json # Application settings
    │   └── FitQuestDb.sqlite # SQLite database file
    │
    ├── FitQuest.Client/     # Blazor WebAssembly
    │   ├── Pages/           # Blazor pages and routing
    │   ├── Components/      # Reusable UI components
    │   ├── Layout/          # Layout components
    │   ├── Services/        # Client-side services
    │   ├── wwwroot/         # Static assets
    │   │   ├── css/         # CSS files (design system)
    │   │   └── index.html   # Main HTML file
    │   ├── Program.cs       # Client startup configuration
    │   └── App.razor        # Root application component
    │
    └── FitQuest.Shared/     # Shared library
        └── Models/          # Shared data models and DTOs
```

### Development Workflow

1. **Start Development**: Use `setup-dev.bat/.sh` for first-time setup
2. **Daily Development**: Use `start-dev.bat/.sh` to start both API and Client
3. **Code Changes**: Both projects support hot reload during development
4. **Database Changes**: Create migrations in the API project
5. **Testing**: Use the Swagger UI at http://localhost:5124/swagger for API testing

### Configuration Files

#### API Configuration

- `appsettings.json`: Production settings
- `appsettings.Development.json`: Development overrides
- Connection strings, CORS policies, and logging configuration

#### Client Configuration

- `Program.cs`: HttpClient configuration, base addresses
- `wwwroot/appsettings.json`: Client-side configuration (if needed)

### Hot Reload Support

Both projects support .NET hot reload:

- **API**: Changes to controllers, services, and most code files
- **Client**: Changes to Razor components, CSS, and C# code
- **CSS**: Changes to `app.css` are reflected immediately

## 🚨 Troubleshooting

### Common Issues

#### "dotnet command not found"

**Problem**: .NET SDK is not installed or not in PATH

**Solution**:

1. Download and install [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
2. Restart your terminal/command prompt
3. Verify installation: `dotnet --version`
4. Ensure the version is 9.0.x or higher

#### "Port already in use" errors

**Problem**: Ports 5124 (API) or 5174 (Client) are already in use

**Solution**:

1. **Windows**: Use `netstat -ano | findstr :5124` to find the process using the port
2. **macOS/Linux**: Use `lsof -i :5124` to find the process
3. Stop the conflicting process or kill it using Task Manager/Activity Monitor
4. Alternatively, modify ports in `Properties/launchSettings.json` files

#### Database connection errors

**Problem**: SQLite database issues or migration failures

**Solution**:

1. **Reset Database**: Delete `FitQuestDb.sqlite` in `src/FitQuest.Api/`
2. **Restart API**: The database will be recreated automatically
3. **Check Permissions**: Ensure the API directory is writable
4. **Migration Issues**: Check the console output for specific Entity Framework errors

#### Build failures

**Problem**: Compilation errors or missing packages

**Solution**:

1. **Clean Solution**: `dotnet clean FitQuest.sln`
2. **Restore Packages**: `dotnet restore FitQuest.sln`
3. **Rebuild**: `dotnet build FitQuest.sln`
4. **Check Errors**: Read the specific error messages for guidance
5. **Clear NuGet Cache**: `dotnet nuget locals all --clear`

#### SignalR connection issues

**Problem**: Real-time features not working, connection failures

**Solution**:

1. **Check Both Services**: Ensure both API and Client are running
2. **Browser Console**: Check for JavaScript errors or connection messages
3. **CORS Configuration**: Verify API allows connections from Client origin
4. **Firewall**: Check if Windows Firewall or antivirus is blocking connections
5. **Browser Refresh**: Try a hard refresh (Ctrl+F5 or Cmd+Shift+R)

#### CSS styling issues

**Problem**: Styles not loading, components look unstyled

**Solution**:

1. **Hard Refresh**: Use Ctrl+F5 (Windows) or Cmd+Shift+R (Mac)
2. **Developer Tools**: Check Network tab for failed CSS requests
3. **CSS File**: Verify `wwwroot/css/app.css` exists and is being served
4. **Browser Cache**: Clear browser cache or use incognito mode
5. **CSS Errors**: Check browser console for CSS parsing errors

#### Application won't start

**Problem**: API or Client fails to start

**Solution**:

1. **Check Prerequisites**: Ensure .NET 9 SDK is installed
2. **Run Setup**: Execute `setup-dev.bat/.sh` to verify configuration
3. **Manual Start**: Try starting each project individually
4. **Check Logs**: Review console output for specific error messages
5. **Port Conflicts**: Ensure ports 5124 and 5174 are available

#### Authentication issues

**Problem**: Login failures, JWT token issues

**Solution**:

1. **Database Reset**: Delete SQLite database to reset user data
2. **Check API Logs**: Review authentication-related error messages
3. **Browser Storage**: Clear browser local storage and cookies
4. **Token Expiry**: Tokens may have expired, try logging in again

### Advanced Troubleshooting

#### Enable Detailed Logging

Add to `appsettings.Development.json` in the API project:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

#### Database Inspection

Use SQLite browser tools to inspect the database:

- **DB Browser for SQLite** (free, cross-platform)
- **SQLite Studio** (free, cross-platform)
- **Visual Studio Code** with SQLite extension

#### Network Debugging

1. **API Health Check**: Visit http://localhost:5124/health
2. **Swagger UI**: Use http://localhost:5124/swagger for API testing
3. **Network Tab**: Use browser developer tools to monitor requests
4. **CORS Headers**: Check if proper CORS headers are present in responses

### Getting Help

If you encounter issues not covered here:

1. **Check Console Output**: Both API and Client provide detailed error messages
2. **Browser Developer Tools**: Check Console and Network tabs for errors
3. **Verify Prerequisites**: Ensure .NET 9 SDK is properly installed
4. **Run Setup Script**: Try running the setup script again
5. **Clean Start**: Delete `bin/` and `obj/` folders, then rebuild

### Development Tips

#### Performance

- **Hot Reload**: Both projects support hot reload for faster development
- **Incremental Builds**: Use `dotnet build --no-restore` for faster builds
- **Parallel Execution**: The start scripts run both projects simultaneously

#### Debugging

- **Visual Studio**: Full debugging support with breakpoints
- **VS Code**: Use C# extension for debugging support
- **Browser DevTools**: Essential for client-side debugging
- **Swagger UI**: Perfect for API endpoint testing

#### Database Management

- **Automatic Migrations**: Migrations run automatically in development
- **Data Seeding**: Initial data is seeded automatically if needed
- **Backup**: Simply copy the `.sqlite` file to backup your data
- **Reset**: Delete the `.sqlite` file to start fresh

#### CSS Development

- **Live Reload**: CSS changes are reflected immediately
- **Browser DevTools**: Use Elements tab to inspect and modify styles
- **CSS Variables**: Modify design tokens in `:root` for global changes
- **Component Isolation**: Each component can have its own `.razor.css` file

## 📝 Features

### Core Functionality

- **User Authentication**: JWT-based secure authentication with ASP.NET Core Identity
- **Goal Management**: Create, update, and track fitness goals with progress monitoring
- **Real-time Updates**: Live notifications and updates via SignalR
- **Dashboard**: Personal fitness dashboard with comprehensive progress tracking
- **Admin Panel**: Administrative features for user and system management

### Technical Features

- **Zero-Configuration Setup**: No Docker or external database required
- **Automatic Database Management**: SQLite with automatic migrations and seeding
- **Modern CSS Design System**: Custom CSS variables-based styling without external frameworks
- **Responsive Design**: Mobile-first design that works on all device sizes
- **Error Handling**: Comprehensive error boundaries and user-friendly error messages
- **Hot Reload**: Development-time hot reload for both API and Client
- **Structured Logging**: Built-in logging with configurable levels
- **Health Checks**: API health monitoring endpoints

### User Experience

- **Progressive Web App**: Can be installed as a PWA on mobile devices
- **Offline Capability**: Basic offline functionality with service workers
- **Accessibility**: WCAG-compliant design with proper ARIA labels
- **Performance**: Optimized loading with minimal dependencies
- **Cross-Platform**: Works on Windows, macOS, and Linux

## 🚀 Deployment

### Development Deployment

The application is designed for easy development deployment:

```bash
# Quick start for development
./setup-dev.sh    # or setup-dev.bat on Windows
./start-dev.sh    # or start-dev.bat on Windows
```

### Production Considerations

For production deployment, consider:

- **Database**: Migrate from SQLite to SQL Server or PostgreSQL
- **Authentication**: Configure production JWT settings
- **HTTPS**: Enable HTTPS and proper SSL certificates
- **Logging**: Configure production logging (e.g., Serilog with external sinks)
- **Environment Variables**: Use environment-specific configuration

## 🤝 Contributing

We welcome contributions! Here's how to get started:

### Development Setup

1. **Fork the repository** on GitHub
2. **Clone your fork**: `git clone <your-fork-url>`
3. **Run setup**: `./setup-dev.sh` (or `.bat` on Windows)
4. **Start development**: `./start-dev.sh` (or `.bat` on Windows)

### Making Changes

1. **Create a feature branch**: `git checkout -b feature/your-feature-name`
2. **Make your changes** following the existing code style
3. **Test thoroughly** using the development environment
4. **Update documentation** if needed
5. **Commit your changes** with clear commit messages

### Code Style Guidelines

- **C#**: Follow standard C# conventions and use nullable reference types
- **CSS**: Use the existing CSS variable system and component-based approach
- **Blazor**: Follow Blazor best practices for component development
- **Database**: Use Entity Framework migrations for schema changes

### Testing

- **Manual Testing**: Use the development environment to test changes
- **API Testing**: Use Swagger UI at http://localhost:5124/swagger
- **Browser Testing**: Test on multiple browsers and device sizes
- **Database Testing**: Verify migrations work correctly

### Pull Request Process

1. **Update documentation** if your changes affect setup or usage
2. **Test your changes** thoroughly in the development environment
3. **Submit a pull request** with a clear description of changes
4. **Respond to feedback** and make requested changes

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [.NET 9](https://dotnet.microsoft.com/) and [Blazor WebAssembly](https://blazor.net/)
- Design system inspired by modern CSS best practices
- Database management powered by [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- Real-time features enabled by [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/)
