using Microsoft.EntityFrameworkCore;
//Añado el using para trabajar con cookies de acceso
using Microsoft.AspNetCore.Authentication.Cookies;
using VentaMusical.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Este es el servicio para conectar a la BD y crear el context
var connectionString = builder.Configuration.GetConnectionString("VentaMusicalContext");
builder.Services.AddDbContext<VentaMusicalContext>(x => x.UseSqlServer(connectionString));

//Este es el servicio para usar cookies para autenticacion:
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Access/Login"; //Pagina que autentifica
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20); //Tiempo que durará la sesion
        option.AccessDeniedPath = "/Access/AccessDenied"; //Pagina donde lo enviaremos cuando expire la sesion

    });

//****************************************

//Este es el servicio para guardar variables de sesion con el id del usuario
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".AdventureWorks.Session";
});
//****************************************

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

//Añadimos el control de cookies al pipeline de carga del proyecto
app.UseAuthentication();
app.UseAuthorization();
//****************************************************************

//Añadimos el uso de cookies session al pipeline de carga del proyecto
app.UseSession();
//****************************************************************


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
