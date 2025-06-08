using Microsoft.EntityFrameworkCore;
using System;
using Hospisim.Data;
using Hospisim.Business.Contracts;
using Hospisim.Business.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IPacienteService, PacienteService>();

var app = builder.Build();


// popular banco
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var db = provider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();          // aplica migrations pendentes
    SeedData.Initialize(provider);  // popula somente se vazio
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
