using ArlequimTech.Core.Data;
using ArlequimTech.Core.Extensions;
using ArlequimTech.Core.Messaging;
using ArlequimTech.Core.Messaging.Interfaces;
using ArlequimTech.Product.Domain.Repositories;
using ArlequimTech.Product.Infrastructure.Repositories;
using ArlequimTech.Stock.Application.Handlers;
using ArlequimTech.Stock.Application.Handlers.Contracts;
using ArlequimTech.Stock.Domain.Repositories;
using ArlequimTech.Stock.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:8090");

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IStockHandler, StockHandler>();
builder.Services.AddScoped<IStockEntryRepository, StockEntryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();

builder.Services.AddPooledDbContextFactory<Context>((serviceProvider, options) =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
        .LogTo(Console.WriteLine, LogLevel.Error);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
