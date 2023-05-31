using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcUzytkownik.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcUzytkownikContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MvcUzytkownikContext") ?? throw new InvalidOperationException("Connection string 'MvcUzytkownikContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Dodanie obsługo sesji
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;//plik cookie jest niedostępny przez skrypt po stronie klienta
    options.Cookie.IsEssential = true;//pliki cookie sesji będą zapisywane dzięki czemu sesje będzie mogła być śledzona podczas nawigacji lub przeładowania strony
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "karnety",
    pattern: "{controller=Karnet}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "uzytkownicy",
    pattern: "{controller=Uzytkownik}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "wejscia",
    pattern: "{controller=Wejscie}/{action=Index}/{id?}");

app.Run();
