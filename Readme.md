# 🚀 Intern Full Stack Developer Test

## 🔽 Clone Repository

```bash
git clone https://github.com/Arachaphon/Arachaphon-Pranworks-Test.git
cd Arachaphon-Pranworks-Test
```

## 📁 Project Structure

| Directory | Description |
|-----------|-------------|
| `frontend/` | Frontend Assessment (Section 2) |
| `backend/` | .NET 10 Minimal API (Section 3) |
| `database/` | Database Schema & Mock Data (Section 4) |

---

## 🛠️ Prerequisites

| Tool | Version | Note |
|------|---------|------|
| Node.js | ≥ 18.x | สำหรับ Frontend (npm/yarn) |
| .NET SDK | 10.0 | สำหรับ Backend Minimal API |
| MySQL | ≥ 8.0 | Database Server |
| Git | ล่าสุด | Clone/Manage source code |

---

## ▶️ How to Run

### 1) Database Setup

```bash
# เข้า MySQL แล้วสร้าง database
mysql -u root -p

CREATE DATABASE shop_db;
EXIT;

# import schema + mock data
mysql -u root -p shop_db < database/schema.sql
```

> แก้ connection string ให้ตรงกับ database ที่สร้าง ที่ `backend/api/appsettings.json`

---

### 2) Backend (.NET 10 Minimal API)

```bash
cd backend/api

# restore dependencies
dotnet restore

# run project
dotnet run
```

- API จะรันที่ `https://localhost:5001` (หรือ port ตามที่ตั้งค่าใน `launchSettings.json`)
- ทดสอบ endpoint ผ่าน Swagger: `https://localhost:5001/swagger`

---

### 3) Frontend (TailAdmin – Tailwind CSS)

```bash
cd frontend

# ติดตั้ง dependencies
npm install

# รัน dev server
npm run start

# หรือ build สำหรับ production
npm run build
```

- Dev server จะรันที่ `http://localhost:5173` (Vite default) หรือ port ที่ระบุใน terminal
- แก้ base URL ของ API ที่เรียกใช้ในไฟล์ config/env ของ frontend ให้ตรงกับ backend (`https://localhost:5001`)

---

# 2. Frontend

## 2.1 HTML Template

| Item | Value |
|------|-------|
| Template | TailAdmin – Free Tailwind CSS Admin Dashboard |
| Source | https://github.com/TailAdmin/tailadmin-free-tailwind-dashboard-template |

---

## 2.2 JavaScript Components

| Feature | File |
|---------|------|
| ApexCharts | `src/js/components/charts/` |
| FullCalendar | `src/calendar.html`<br>`src/js/components/calendar-init.js` |
| jsVectorMap | `src/js/components/map-01.js` |
| Flatpickr Date Range Picker | `src/js/index.js`<br>`src/partials/datepicker.html` |
| Dropzone File Upload | `src/js/index.js` |
| Sidebar Toggle | `src/partials/header.html`<br>`src/partials/sidebar.html` |
| Dark Mode Toggle | `src/*.html`<br>`src/partials/header.html` |

---

## 2.3 Student Current Location

**Files**

```
src/student-location.html
src/js/components/student-location.js
```

---

## 2.4 Image Gallery

- Image Grid
- Image Slider

---

# 3. Backend (.NET 10 Minimal API)

## 3.1 Project Setup

| Description | File |
|------------|------|
| .NET 10 Minimal API | `backend/api/api.csproj` |
| Program Configuration | `backend/api/Program.cs` |

---

## 3.2 Database Connection

| Description | File |
|------------|------|
| AddDbContext + MySQL | `backend/api/Program.cs` |
| DbContext | `backend/api/Data/AppDbContext.cs` |
| Connection String | `backend/api/appsettings.json` |

---

## 3.3 REST API

### GET

| Endpoint | Parameters | Response Fields |
|----------|------------|----------------|
| `/api/products/search` | keyword, minPrice, maxPrice, isAvailable | 7 |

### POST

| Endpoint | Parameters | Response Fields |
|----------|------------|----------------|
| `/api/members` | firstName, lastName, email, phone | 6 |
| `/api/orders` | memberId, productId, quantity, shippingAddress | 8 |

---

## 3.4 Debug & Watch

Endpoint

```
GET /api/debug/order-flow
```

- 5 Breakpoints
- 5 Watch Variable Groups

---

# 4. Database

## 4.1 Database Design

### Files

```
database/schema.sql

backend/api/Data/AppDbContext.cs

backend/api/Models/
├── Member.cs
├── Product.cs
└── Order.cs
```

### Requirements

| Requirement | Status |
|------------|:------:|
| ≥ 3 Tables | ✅ |
| Running Number PK | ✅ |
| Foreign Keys | ✅ |
| ≥ 5 Columns / Table | ✅ |
| INT | ✅ |
| NVARCHAR | ✅ |
| BOOLEAN | ✅ |
| DATETIME | ✅ |

---

## 4.2 Mock Data

| Table | Records |
|-------|:-------:|
| Members | ✅ 50 |
| Products | ✅ 50 |
| Orders | ✅ 50 |
