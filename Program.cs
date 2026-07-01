using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShepherdsPiesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ShepherdsPiesDbConnectionString")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPizzaRepository, PizzaRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<ICheeseOptionRepository, CheeseOptionRepository>();
builder.Services.AddScoped<ISauceOptionRepository, SauceOptionRepository>();
builder.Services.AddScoped<IToppingRepository, ToppingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
