using lab3_11;
using lab3_11.api;
using lab3_11.api.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<TcpServer>();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<RoomService>();

builder.Services.AddScoped<Worker>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Ініціалізація бази даних
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

await host.RunAsync();