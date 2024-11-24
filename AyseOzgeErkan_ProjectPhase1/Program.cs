using Microsoft.EntityFrameworkCore;
using BLL.DAL;
using BLL.Services;  // Adjust this namespace based on your project structure

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register application services
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<DirectorService>();

// Register AppDbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Show detailed errors in development mode
}
else
{
    app.UseExceptionHandler("/Home/Error");  // Friendly error page in production
    app.UseHsts();  // Enforce HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Avoid printing connection string in production for security reasons
if (app.Environment.IsDevelopment())
{
    Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection")); // Only log connection string in development
}

app.Run();