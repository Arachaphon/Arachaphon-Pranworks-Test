# Intern Full Stack Developer Test

## โครงสร้างโปรเจค
- `/frontend` - ข้อ 2 (TailAdmin Template)
    2.1 /frontend
    Template : TailAdmin - Free Tailwind Admin Dashboard Template
    Source : https://github.com/TailAdmin/tailadmin-free-tailwind-dashboard-template
    2.2
    *ApexCharts*
    File: src/js/components/charts

    *FullCalendar*
    File: src/calendar.html , src/js/components/calendar-init.js

    *jsVectorMap*
    File:  src/js/components/map-01.js

    *Flatpickr Date Range Picker*
    File:  src/js/index.js, src/partials/datepicker.html

    *Dropzone File Upload*
    File:  src/js/index.js

    *Sidebar Toggled*
    File:  src/partials/header.html, src/partials/sidebar.html

    *Dark Mode Toggle*
    File:  src/*.html , src/partials/header.html

    2.3 File : src/student-location.html,src/js/components/student-location.js

    2.4 Image Grid + Slider

- `/backend` - ข้อ 3 (.NET Core API)
    3.1 Project (.NET 10 Minimal API)
        File: backend/api/api.csproj (TargetFramework: net10.0)
        File: backend/api/Program.cs (Minimal API, CORS, Swagger)

    3.2 Database Connection
        File: backend/api/Program.cs:23-27 (AddDbContext with UseMySql)
        File: backend/api/Data/AppDbContext.cs (DbSet Members, Products, Orders)
        File: backend/api/appsettings.json:10 (ConnectionString: shop_db)

    3.3 GET/POST API
        GET /api/products/search (4 params: keyword, minPrice, maxPrice, isAvailable | 7 response fields)
        File: backend/api/Program.cs:64-97

        POST /api/members (4 body params: firstName, lastName, email, phone | 6 response fields)
        File: backend/api/Program.cs:100-143

        POST /api/orders (4 body params: memberId, productId, quantity, shippingAddress | 8 response fields)
        File: backend/api/Program.cs:221-251

    3.4 Debug + Add Watch
        GET /api/debug/order-flow (5 breakpoints, 5 watch variable groups)
        File: backend/api/Program.cs:146-218

- `/database` - ข้อ 4 (DB Schema + Mock Data)
    4.1 DB Design
        File: database/schema.sql (3 tables: members, products, orders)
        File: backend/api/Data/AppDbContext.cs:14-66 (EF Core Fluent API mapping)
        File: backend/api/Models/Member.cs, Product.cs, Order.cs (Entity Models)

        - 3 tables ✓ (members, products, orders)
        - PK running-number (AUTO_INCREMENT) ✓
        - FK เชื่อม 3 ตาราง (orders.member_id → members, orders.product_id → products) ✓
        - ≥5 columns per table: members(7), products(7), orders(8) ✓
        - Data types: INT, VARCHAR (NVARCHAR equivalent + utf8mb4), BOOLEAN, DATETIME ✓

    4.2 Mock Data
        File: database/mock_data.sql (50 records per table)
        - members: 50 records ✓
        - products: 50 records ✓
        - orders: 50 records ✓
