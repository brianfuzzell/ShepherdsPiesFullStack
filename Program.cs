using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;
using ShepherdsPiesControllers.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ShepherdsPiesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ShepherdsPiesDbConnectionString")));

builder.Services.AddIdentity<Employee, IdentityRole>()
    .AddEntityFrameworkStores<ShepherdsPiesDbContext>()
    .AddDefaultTokenProviders();

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

builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await SeedIdentityDataAsync(app);

app.Run();

static async Task SeedIdentityDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();

    foreach (var role in new[] { "Employee", "Manager" })
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    await EnsureSeedEmployeeAsync(userManager, "employee@shepherdspies.com", "Pat", "Employee", "Employee123!", "Employee");
    await EnsureSeedEmployeeAsync(userManager, "manager@shepherdspies.com", "Morgan", "Manager", "Manager123!", "Manager");
}

static async Task EnsureSeedEmployeeAsync(UserManager<Employee> userManager, string email, string firstName, string lastName, string password, string role)
{
    if (await userManager.FindByEmailAsync(email) is not null)
    {
        return;
    }

    var employee = new Employee
    {
        UserName = email,
        Email = email,
        FirstName = firstName,
        LastName = lastName
    };

    var result = await userManager.CreateAsync(employee, password);
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(employee, role);
    }
}
