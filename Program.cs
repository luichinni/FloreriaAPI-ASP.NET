using FloreriaAPI_ASP.NET.Models;
using FloreriaAPI_ASP.NET.Repository;
using FloreriaAPI_ASP.NET.Services;
using FloreriaAPI_ASP.NET.Web;
using FloreriaAPI_ASP.NET.Web.Seeds;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.AddDbContext<FloreriaContext>(op => op.UseSqlite("data source=Floreria.sqlite"));

builder.Services.AddCors();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddCookie(IdentityConstants.ApplicationScheme);

builder.Services.AddScoped<IAuthorizationHandler, RoleClaimHandler>();

// Crea todos los permisos como politicas
// https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims?view=aspnetcore-8.0
foreach (var p in PermisosEnum.Todos)
{
    builder.Services.AddAuthorization(
        op => op.AddPolicy(
            p,
            policy => policy.AddRequirements(new RoleClaimRequirement(PermisosEnum.PermisoType, p))
        )
    );
}
// REVISAR COOKIES Y LOGIN
builder.Services.AddIdentityCore<FloreriaUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FloreriaContext>()
    .AddSignInManager();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<IFlowerService, FlowerService>();
builder.Services.AddScoped<IFlowerRepository, FlowerRepository>();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await Seeds.Run(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.UseCors(options =>
    options.WithOrigins("http://localhost:5228")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);

app.Use(async (context, next) =>
{
    await next();
    Console.WriteLine($"{context.Request.Method} {context.Request.Path} {context.Response.StatusCode}");
});

app.MapControllers();

app.Run();
