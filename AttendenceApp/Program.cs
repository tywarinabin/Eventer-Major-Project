using AttendenceApp.DatabaseContext;
using AttendenceApp.Logging;
using AttendenceApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
LoggingClass logging = LoggingClass.GetInstance;

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddWebOptimizer();

// Register DbContext
builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    var method = context.Request.Method;
    var path = context.Request.Path;
    var ip = context.Connection.RemoteIpAddress?.ToString();
    var timestamp = DateTime.UtcNow;

    logging.WriteLog($"[{timestamp}] {method} {path} from {ip}");

    await next(); 
});
app.Use(async (context, next) =>
{
    await next(); // Process the request

    // If the response status code is 404, redirect to the 404 page
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Request.Path = "/404"; // Route to the 404 page
        await next();
    }
});
app.UseWebOptimizer();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
