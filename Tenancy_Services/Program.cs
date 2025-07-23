using Application_Tenancy.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using Tenancy_Persistence.Db_Con;
using Tenancy_Services.C_MiddleWare;
using Tenancy_Shared.Enums;
using Tenancy_Shared.TenantProvider;
using Tenancy_Shared.ConnectionManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// 🌐 Tenant provider and connection resolver
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<IConnectionResolver, ConnectionResolver>();

// 🔗 Scoped DB connection per tenant
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var resolver = sp.GetRequiredService<IConnectionResolver>();
    return new NpgsqlConnection(resolver.GetCurrentConnectionString());
});

builder.Services.AddScoped<UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hybrid Tenancy API", Version = "v1" });
    c.OperationFilter<TenantHeaderOperationFilter>();
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hybrid Tenancy Service is running ✅");

app.Run();
