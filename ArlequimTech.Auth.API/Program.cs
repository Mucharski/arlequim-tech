using ArlequimTech.Auth.Application.Handlers;
using ArlequimTech.Auth.Application.Handlers.Contracts;
using ArlequimTech.Auth.Application.Services;
using ArlequimTech.Auth.Application.Services.Contracts;
using ArlequimTech.Auth.Domain.Repositories;
using ArlequimTech.Auth.Infrastructure.Repositories;
using ArlequimTech.Core.Data;
using ArlequimTech.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// Services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthHandler, AuthHandler>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddPooledDbContextFactory<Context>((serviceProvider, options) =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .LogTo(Console.WriteLine, LogLevel.Error);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();