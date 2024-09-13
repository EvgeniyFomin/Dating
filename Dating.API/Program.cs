using Dating.API.Extensions;
using Dating.API.Services;
using Dating.API.Services.Interfaces;
using Dating.DAL.Context;
using Dating.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddIdentityServices(builder.Configuration);

// API stuff
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// DAL stuff
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(
        builder.Configuration.GetConnectionString("SQLite"),
        options => options.MigrationsAssembly("Dating.DAL"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x.AllowAnyOrigin()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200", "https://localhost:4200"));
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

app.Run();
