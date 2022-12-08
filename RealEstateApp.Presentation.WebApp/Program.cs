using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Identity.Entities;
using RealEstateApp.Infrastructure.Identity.Seeds;
using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Shared;
using RealEstateApp.Presentation.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(userManager, roleManager);
        await DefaultAdminUser.SeedAsync(userManager, roleManager);
        await DefaultDevUser.SeedAsync(userManager, roleManager);
        await DefaultClientUser.SeedAsync(userManager, roleManager);
        await DefaultAgentUser.SeedAsync(userManager, roleManager);
    }

    catch (Exception ex)
    {
        Console.Write(ex);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
