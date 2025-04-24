using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔧 1. Підключення контексту бази даних
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// або:
// options.UseSqlServer(...); // Якщо SQL Server

// 🔧 2. MVC + сесія + куки-автентифікація
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();

// 🔧 3. DI-сервіси
builder.Services.AddScoped<CartService>();
builder.Services.AddSingleton<ReviewService>();
builder.Services.AddScoped<DataAccessor>();


var app = builder.Build();

// 🧱 Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// 📍 Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new Category { Name = "Іграшки", ImageUrl = "toys.jpg" },
            new Category { Name = "Канцелярія", ImageUrl = "office.jpeg" },
            new Category { Name = "Косметика", ImageUrl = "beauty.jpg" },
            new Category { Name = "Продукти", ImageUrl = "food.jpg" },
            new Category { Name = "Електроніка", ImageUrl = "electronics.jpg" }
        );
        context.SaveChanges();
    }
}


app.Run();

