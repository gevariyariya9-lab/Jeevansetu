using Microsoft.AspNetCore.Authentication.Cookies;
using JeevanSetu.Data;
using JeevanSetu.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<
    Microsoft.AspNetCore.Identity.IPasswordHasher<JeevanSetu.Models.Donor>,
    Microsoft.AspNetCore.Identity.PasswordHasher<JeevanSetu.Models.Donor>>();
builder.Services.AddScoped<
    Microsoft.AspNetCore.Identity.IPasswordHasher<JeevanSetu.Models.Hospital>,
    Microsoft.AspNetCore.Identity.PasswordHasher<JeevanSetu.Models.Hospital>>();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient<MlPredictionService>(client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:5000/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
