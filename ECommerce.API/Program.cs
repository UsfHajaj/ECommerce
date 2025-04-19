using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Config;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce API",
        Version = "v1",
        Description = "API documentation for ECommerce system"
    });
});


var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API V1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
    });
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Apply migrations if needed
    context.Database.Migrate();

    // Seed the data
    await ApplicationDbSeedData.SeedAsync(context);
    await RoleSeeder.SeedRolesAsync(services);
}

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

