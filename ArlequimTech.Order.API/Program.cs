using ArlequimTech.Core.Data;
using ArlequimTech.Core.Extensions;
using ArlequimTech.Order.Application.Handlers;
using ArlequimTech.Order.Application.Handlers.Contracts;
using ArlequimTech.Order.Domain.Repositories;
using ArlequimTech.Order.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:8095");

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IOrderHandler, OrderHandler>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddDbContext<Context>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .LogTo(Console.WriteLine, LogLevel.Error);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();