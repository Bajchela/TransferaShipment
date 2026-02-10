using Microsoft.EntityFrameworkCore;
using Shipments.WorkerService;
using Shipments.WorkerService.Messaging;
using Shipments.WorkerService.Messaging.AzureServiceBus;
using Shipments.WorkerService.Persistence.Interface;
using Shipments.WorkerService.Persistence.Worker;
using Shipments.WorkerService.Processing;
using Shipments.WorkerService.Processing.Interface;
using Shipments.WorkerService.Storage.AzureBlob;
using Shipments.WorkerService.Storage.Interface;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ServiceBusOptions>(builder.Configuration.GetSection("Azure:ServiceBus"));
builder.Services.Configure<BlobOptions>(builder.Configuration.GetSection("Azure:Blob"));

builder.Services.AddDbContext<WorkerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IShipmentStatusUpdater, ShipmentStatusUpdater>();
builder.Services.AddSingleton<IMessageReceiver, ServiceBusMessageReceiver>();
builder.Services.AddScoped<IBlobDownloader, AzureBlobDownloader>();
builder.Services.AddScoped<IShipmentDocumentProcessor, ShipmentDocumentProcessor>();

builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.Run();
