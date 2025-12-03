#region
using AutoMapper;
using Fiap.Web.Alunos.Data.Contexts;
using Fiap.Web.Alunos.Data.Repository;
using Fiap.Web.Alunos.Logging;
using Fiap.Web.Alunos.Models;
using Fiap.Web.Alunos.Services;
using Fiap.Web.Alunos.ViewModels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<DatabaseContext>(
    opt => opt.UseOracle(connectionString).EnableSensitiveDataLogging(true)
);

#endregion

#region Registro IserviceCollection

builder.Services.AddSingleton<ICustomLogger, MockLogger>();

#endregion

#region Registro de Serviços e Repositorio

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

#endregion

#region AutoMapper

// Registra o AutoMapper e busca por perfis no assembly atual
builder.Services.AddAutoMapper(cfg => {}, typeof(Program).Assembly);

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();