using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestEF;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration["ConnectionString"]));
builder.Services.AddHostedService<Worker>();

using var host = builder.Build();
await host.RunAsync();