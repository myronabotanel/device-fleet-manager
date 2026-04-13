# 📱 Device Fleet Manager

A full-stack web application for managing a company's mobile device inventory. Built with **ASP.NET Core Web API (.NET 8)**, **Angular**, and **MongoDB**.

---

## ✨ Features

- **Device Management** — Create, view, edit, and delete devices with full details
- **User Authentication** — Register and login with JWT-based authentication
- **Device Assignment** — Users can assign/unassign devices to themselves
- **AI Description Generator** — Automatically generate device descriptions using the Gemini API
- **Free-text Search** — Relevance-ranked search across device name, manufacturer, processor, and RAM
- **Form Validation** — All forms validate required fields before submission

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | C#, ASP.NET Core Web API, .NET 8 |
| Frontend | Angular 17+ |
| Database | MongoDB |
| Auth | JWT (JSON Web Tokens) |
| AI | Google Gemini API |
| Version Control | Git / GitHub |

---

## ✅ Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or later) + npm
- [Angular CLI](https://angular.io/cli): `npm install -g @angular/cli`
- [MongoDB](https://www.mongodb.com/try/download/community) (local) or a [MongoDB Atlas](https://www.mongodb.com/atlas) connection string

---

## ⚙️ Configuration

### Backend — `appsettings.json`

In `DeviceFleetManager.API/appsettings.json`, set your values:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "DeviceFleetManager"
  },
  "Jwt": {
    "Secret": "your_jwt_secret_key_here",
    "Issuer": "DeviceFleetManager",
    "Audience": "DeviceFleetManagerUsers"
  },
  "Gemini": {
    "ApiKey": "your_gemini_api_key_here"
  }
}
```

---

## 🚀 Running the Project Locally

### 1. Clone the repository

```bash
git clone https://github.com/your-username/DeviceFleetManager.git
cd DeviceFleetManager
```

### 2. Seed the database (optional but recommended)

The `seed/` folder contains MongoDB scripts to create collections and populate them with dummy data.

```bash
cd seed
mongosh < seed_devices.js
mongosh < seed_users.js
```

> Scripts are idempotent — safe to run multiple times.

### 3. Start the Backend

```bash
cd DeviceFleetManager.API
dotnet run
```

The API will be available at: `http://localhost:5019`

You should see:
```
✅ Conexiune MongoDB reusita!
Now listening on: http://localhost:5019
```

### 4. Start the Frontend

```bash
cd DeviceFleetManager.Frontend
npm install
ng serve
```

The app will be available at: `http://localhost:4200`

---

## 📖 API Endpoints

### Auth — `/api/auth` (public)
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and receive JWT token |

### Devices — `/api/device` (🔒 requires JWT)
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/device` | Get all devices |
| GET | `/api/device/{id}` | Get device by ID |
| POST | `/api/device` | Create a new device |
| PUT | `/api/device/{id}` | Update a device |
| DELETE | `/api/device/{id}` | Delete a device |
| PUT | `/api/device/{id}/assign` | Assign device to current user |
| PUT | `/api/device/{id}/unassign` | Unassign device from current user |
| GET | `/api/device/search?q={query}` | Free-text search with relevance ranking |

### Users — `/api/user` (public)
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/user` | Get all users |
| GET | `/api/user/{id}` | Get user by ID |
| POST | `/api/user` | Create a new user |
| PUT | `/api/user/{id}` | Update a user |
| DELETE | `/api/user/{id}` | Delete a user |

### AI — `/api/ai` (🔒 requires JWT)
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/ai/generate-description` | Generate device description using Gemini |

---

## 📁 Project Structure

```
DeviceFleetManager/
├── DeviceFleetManager.API/        # ASP.NET Core Web API
│   ├── Controllers/               # API controllers
│   ├── Models/                    # Device, User models
│   ├── Repositories/              # MongoDB data access
│   ├── Services/                  # Business logic
│   └── appsettings.json           # Configuration (not committed)
├── DeviceFleetManager.Frontend/   # Angular application
│   ├── src/app/
│   │   ├── components/            # Device list, detail, form
│   │   ├── services/              # HTTP services
│   │   └── guards/                # Auth guard
└── seed/                          # MongoDB seed scripts
```

---

## 🔐 Authentication Flow

1. Register at `/register` with name, email, and password
2. Login at `/login` — a JWT token is stored in `localStorage`
3. All device endpoints require a valid `Authorization: Bearer <token>` header
4. The auth guard redirects unauthenticated users to `/login`

---

## 🤖 AI Description Generator

On the device create/edit form, click **"Generate Description"** to automatically generate a human-readable description based on the device's name, manufacturer, OS, type, RAM, and processor — powered by the Google Gemini API.
