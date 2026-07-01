using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos;
using api.Models;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Api v1");
    });
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.MapGet("/api/members", async (AppDbContext db) =>
    await db.Members.ToListAsync());

app.MapGet("/api/members/{id}", async (int id, AppDbContext db) =>
    await db.Members.FindAsync(id) is { } member ? Results.Ok(member) : Results.NotFound());

app.MapGet("/api/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapGet("/api/products/{id}", async (int id, AppDbContext db) =>
    await db.Products.FindAsync(id) is { } product ? Results.Ok(product) : Results.NotFound());

app.MapGet("/api/orders", async (AppDbContext db) =>
    await db.Orders.Include(o => o.Member).Include(o => o.Product).ToListAsync());

app.MapGet("/api/orders/{id}", async (int id, AppDbContext db) =>
    await db.Orders.Include(o => o.Member).Include(o => o.Product).FirstOrDefaultAsync(o => o.OrderId == id) is { } order
        ? Results.Ok(order) : Results.NotFound());

// === 3.3: GET with 4 query params, response >= 5 fields ===
app.MapGet("/api/products/search", async (
    string? keyword,
    decimal? minPrice,
    decimal? maxPrice,
    bool? isAvailable,
    AppDbContext db) =>
{
    var query = db.Products.AsQueryable();

    if (!string.IsNullOrWhiteSpace(keyword))
        query = query.Where(p => p.ProductName.Contains(keyword) || (p.Description != null && p.Description.Contains(keyword)));

    if (minPrice.HasValue)
        query = query.Where(p => p.Price >= minPrice.Value);

    if (maxPrice.HasValue)
        query = query.Where(p => p.Price <= maxPrice.Value);

    if (isAvailable.HasValue)
        query = query.Where(p => p.IsAvailable == isAvailable.Value);

    var results = await query.Select(p => new
    {
        p.ProductId,
        p.ProductName,
        p.Description,
        p.Price,
        p.Stock,
        p.IsAvailable,
        p.CreatedAt
    }).ToListAsync();

    return Results.Ok(results);
});

// === 3.3: POST with 4 body params, response >= 6 fields ===
app.MapPost("/api/members", async (CreateMemberRequest req, AppDbContext db, ILogger<Program> logger) =>
{
    Debug.WriteLine("=== DEBUG [POST /api/members] Start ===");
    Debug.Indent();
    Debug.WriteLine($"Received: FirstName={req.FirstName}, LastName={req.LastName}, Email={req.Email}");

    logger.LogInformation("Creating member: {FirstName} {LastName} ({Email})", req.FirstName, req.LastName, req.Email);

    var member = new Member
    {
        FirstName = req.FirstName,
        LastName = req.LastName,
        Email = req.Email,
        Phone = req.Phone,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

    // Breakpoint here — Watch: member (FirstName, LastName, Email, IsActive)
    Debug.WriteLine($"New member object created — ID: {member.MemberId}, Active: {member.IsActive}");

    db.Members.Add(member);
    await db.SaveChangesAsync();

    // After SaveChanges — member.MemberId is now populated
    Debug.WriteLine($"Saved — MemberId={member.MemberId}");
    Debug.Unindent();
    Debug.WriteLine("=== DEBUG [POST /api/members] End ===");

    return Results.Created($"/api/members/{member.MemberId}", new CreateMemberResponse
    {
        MemberId = member.MemberId,
        FirstName = member.FirstName,
        LastName = member.LastName,
        Email = member.Email,
        IsActive = member.IsActive,
        CreatedAt = member.CreatedAt
    });
});

// === 3.4: Debug demo — step-by-step object state with watch ===
app.MapGet("/api/debug/order-flow", async (int memberId, int productId, int quantity, string? address, AppDbContext db, ILogger<Program> logger) =>
{
    Debug.WriteLine("");
    Debug.WriteLine("╔══════════════════════════════════════════════╗");
    Debug.WriteLine("║  DEBUG DEMO — Order Flow with Watch         ║");
    Debug.WriteLine("╚══════════════════════════════════════════════╝");
    Debug.WriteLine("");

    // ── STEP 1: Validate input ──
    Debug.WriteLine("─ Step 1: Validate Request ──");
    Debug.WriteLine($"Params: memberId={memberId}, productId={productId}, qty={quantity}");
    var validatedAddress = string.IsNullOrWhiteSpace(address) ? "No address provided" : address;
    Debug.WriteLine($"  > address = \"{validatedAddress}\"");
    // Breakpoint — Watch: memberId, productId, quantity, validatedAddress

    // ── STEP 2: Load Member ──
    Debug.WriteLine("─ Step 2: Lookup Member ──");
    var member = await db.Members.FindAsync(memberId);
    Debug.WriteLine($"  > member found = {member != null}");
    // Breakpoint — Watch: member (expand to see FirstName, LastName, Email, IsActive)
    if (member == null)
        return Results.NotFound(new { error = "Member not found" });

    // ── STEP 3: Load Product ──
    Debug.WriteLine("─ Step 3: Lookup Product ──");
    var product = await db.Products.FindAsync(productId);
    Debug.WriteLine($"  > product found = {product != null}");
    // Breakpoint — Watch: product
    if (product == null)
        return Results.NotFound(new { error = "Product not found" });

    // ── STEP 4: Build Order object ──
    Debug.WriteLine("─ Step 4: Build Order Object ──");
    var order = new Order
    {
        MemberId = memberId,
        ProductId = productId,
        Quantity = quantity,
        ShippingAddress = validatedAddress,
        TotalAmount = product.Price * quantity,
        IsPaid = false,
        OrderDate = DateTime.UtcNow
    };
    // Breakpoint — Watch: order (expand to see all 8 fields)
    // Add Watch: order.TotalAmount, order.Quantity, product.Price
    Debug.WriteLine($"  > order.TotalAmount = {product.Price} x {quantity} = {order.TotalAmount}");
    Debug.WriteLine($"  > order before save: Id={order.OrderId}, Paid={order.IsPaid}, Date={order.OrderDate}");

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    // ── STEP 5: After save ──
    Debug.WriteLine("─ Step 5: After SaveChanges ──");
    Debug.WriteLine($"  > order.OrderId = {order.OrderId} (auto-generated by DB)");
    Debug.WriteLine($"  > order.OrderDate = {order.OrderDate}");
    // Breakpoint — Watch: order (see that OrderId is now populated)

    logger.LogInformation("Order {OrderId} created for Member {MemberId}, total {TotalAmount}",
        order.OrderId, memberId, order.TotalAmount);

    Debug.WriteLine("");
    Debug.WriteLine("╔══════════════════════════════════════════════╗");
    Debug.WriteLine("║  DEBUG DEMO COMPLETE                        ║");
    Debug.WriteLine("╚══════════════════════════════════════════════╝");

    return Results.Created($"/api/orders/{order.OrderId}", new
    {
        order.OrderId,
        MemberName = $"{member.FirstName} {member.LastName}",
        ProductName = product.ProductName,
        order.Quantity,
        UnitPrice = product.Price,
        order.TotalAmount,
        order.ShippingAddress,
        order.IsPaid,
        order.OrderDate
    });
});

// === 3.3: POST with 4 body params, response >= 8 fields ===
app.MapPost("/api/orders", async (CreateOrderRequest req, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(req.ProductId)
        ?? throw new BadHttpRequestException("Product not found");

    var order = new Order
    {
        MemberId = req.MemberId,
        ProductId = req.ProductId,
        Quantity = req.Quantity,
        ShippingAddress = req.ShippingAddress,
        TotalAmount = product.Price * req.Quantity,
        IsPaid = false,
        OrderDate = DateTime.UtcNow
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    return Results.Created($"/api/orders/{order.OrderId}", new CreateOrderResponse
    {
        OrderId = order.OrderId,
        MemberId = order.MemberId,
        ProductId = order.ProductId,
        Quantity = order.Quantity,
        ShippingAddress = order.ShippingAddress,
        TotalAmount = order.TotalAmount,
        IsPaid = order.IsPaid,
        OrderDate = order.OrderDate
    });
});

app.Run();
