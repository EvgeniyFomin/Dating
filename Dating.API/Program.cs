using Dating.API.Extensions;
using Dating.API.Middleware;
using Dating.API.Services;
using Dating.API.Services.CloudinaryService;
using Dating.API.Services.Interfaces;
using Dating.Core.Models;
using Dating.Core.Models.Identity;
using Dating.DAL.Context;
using Dating.DAL.Repositories;
using Dating.DAL.Repositories.Interfaces;
using Dating.DAL.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddIdentityServices(builder.Configuration);

// API stuff
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, CloudinaryService>();
builder.Services.AddScoped<ILikesService, LikesService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// DAL stuff
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ILikesRepository, LikesRepository>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(
        builder.Configuration.GetConnectionString("SQLite"),
        options => options.MigrationsAssembly("Dating.DAL"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dating API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error during the migration");
}

app.Run();
