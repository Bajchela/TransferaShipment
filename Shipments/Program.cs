using Ads.Application.Interfaces.Auth;
using Ads.Infrastructure.Repositories.Auth;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Shipments.Application.Mapping;
using Shipments.Application.Services;
using Shipments.Contracts.Interfaces.Auth;
using Shipments.Contracts.Interfaces.Repositories;
using Shipments.Contracts.Interfaces.Services;
using Shipments.Contracts.Messaging;
using Shipments.Contracts.Storage.Blobs;
using Shipments.Infrastructure.Messaging;
using Shipments.Infrastructure.Middleware;
using Shipments.Infrastructure.Persistance;
using Shipments.Infrastructure.Repositories;
using Shipments.Infrastructure.Security;
using Shipments.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("DefaultConnection is missing in configuration.");

builder.Services.AddDbContext<ShipmentsDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddAutoMapper(typeof(ShipmentProfile));

builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IShipmentDocumentUploadService, ShipmentDocumentUploadService>();
builder.Services.AddScoped<IShipmentDocumentUploadRepository, ShipmentDocumentUploadRepository>();
builder.Services.AddScoped<IShipmentDocumentRepository,ShipmentDocumentRepository>();


#region Auth Services and Repositories
builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
#endregion

var sbConn = builder.Configuration["Azure:ServiceBus:ConnectionString"];
var queueName = builder.Configuration["Azure:ServiceBus:DocumentsQueueName"] ?? "documents-to-process";

if (string.IsNullOrWhiteSpace(sbConn))
{
    builder.Services.AddScoped<IDocumentQueuePublisher, FakeDocumentQueuePublisher>();
}
else
{
    builder.Services.AddSingleton(new ServiceBusClient(sbConn));
    builder.Services.AddScoped<IDocumentQueuePublisher>(sp =>
    {
        var client = sp.GetRequiredService<ServiceBusClient>();
        return new ServiceBusDocumentQueuePublisher(client, queueName);
    });
}

var blobConn = builder.Configuration["Azure:Blob:ConnectionString"];

if (string.IsNullOrWhiteSpace(blobConn))
{
    builder.Services.AddScoped<IBlobStorage, FakeBlobStorage>();
}
else
{
    builder.Services.AddSingleton(new BlobServiceClient(blobConn));
    builder.Services.AddScoped<IBlobStorage, AzureBlobStorage>();
}

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.Use(async (context, next) =>
{
    const string headerName = "X-Correlation-Id";

    var correlationId =
        context.Request.Headers.TryGetValue(headerName, out var value)
            ? value.ToString()
            : Guid.NewGuid().ToString();

    context.Items[headerName] = correlationId;

    context.Response.Headers[headerName] = correlationId;

    await next();
});

using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<ShipmentsDbContext>();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }
