using InventoryDB.Models.Database;
using InventoryDB.Repository.Inventories;
using InventoryDB.Repository.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options =>
{
    //Get Connection string from the dbcontext
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventorySystem"));
    //to remove the string and put it directly in the program instead of getting connection string
    //options.UseSqlServer("Server=EA611-13;Database=RegistrationSystem;Trusted_Connection=true;TrustServerCertificate=true");

});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Account/Login";
                   options.AccessDeniedPath = "/Account/AccessDenied";
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                   options.SlidingExpiration = true;

                   /* Given path 
                   https://localhost:7112/Accounts/Login?ReturnUrl=%2FHome%2FLogin */

                   //TODO: Secured Controllers 
               });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();  

app.UseRouting();

app.UseSession();     

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inventory}/{action=Index}/{id?}");

app.Run();
