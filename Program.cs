using AppMexaba.Models;
using Microsoft.EntityFrameworkCore;
using AppMexaba.Services;
using AppMexaba.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ? Agrega el filtro de sesión global
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<SessionAuthorizeAttribute>();
});

// ? Inyección de dependencias
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IOrigenService, OrigenService>();
builder.Services.AddScoped<VentasVipService>();
builder.Services.AddScoped<IAbastoPorCapaService, AbastoPorCapaService>();
builder.Services.AddScoped<IVentaCFRService, VentaCFRService>();

// ? Configuración de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ? Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();         // La sesión debe ir antes de Authorization
app.UseAuthorization();

// ? Ruta por defecto para cubrir todos los controladores

app.MapControllerRoute(
    name: "ventasCFR",
    pattern: "VentasCFR/{action=Index}/{id?}",
    defaults: new { controller = "VentasCFR" });

app.MapControllerRoute(
    name: "catalogo",
    pattern: "Catalogo/{action=Index}/{id?}",
    defaults: new { controller = "Catalogo" });

app.MapControllerRoute(
    name: "origen",
    pattern: "Origen/{action=Index}/{id?}",
    defaults: new { controller = "Origen" });

app.MapControllerRoute(
    name: "sucursales",
    pattern: "Sucursales/{action=Index}/{id?}",
    defaults: new { controller = "Sucursales" });

app.MapControllerRoute(
    name: "inventarioSucursal",
    pattern: "InventarioSucursal/{action=Index}/{id?}",
    defaults: new { controller = "InventarioSucursal" });

app.MapControllerRoute(
    name: "abastoPorCapa",
    pattern: "AbastoPorCapa/{action=Index}/{id?}",
    defaults: new { controller = "AbastoPorCapa" });

app.MapControllerRoute(
    name: "ventasVip",
    pattern: "VentasVip/{action=Index}/{id?}",
    defaults: new { controller = "VentasVip" });

app.MapControllerRoute(
    name: "kioskoMexaba",
    pattern: "KioskoMexaba/{action=Index}/{id?}",
    defaults: new { controller = "KioskoMexaba" });

app.MapControllerRoute(
    name: "home",
    pattern: "Home/{action=Index}/{id?}",
    defaults: new { controller = "Home" });

app.MapControllerRoute(
    name: "login",
    pattern: "Login/{action=Login}/{id?}",
    defaults: new { controller = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");


app.Run();
