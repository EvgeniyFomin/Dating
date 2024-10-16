using Dating.API.Extensions;
using Dating.API.Middleware;
using Dating.API.Services;
using Dating.API.Services.CloudinaryService;
using Dating.API.Services.Interfaces;
using Dating.DAL.Context;
using Dating.DAL.Repositories;
using Dating.DAL.Repositories.Interfaces;
using Dating.DAL.Seed;
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
builder.Services.AddScoped<LogUserActivity>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

// DAL stuff
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ILikesRepository, LikesRepository>();
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

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error during the migration");
}

app.Run();
