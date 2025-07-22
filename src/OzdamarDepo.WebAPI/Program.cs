using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using OzdamarDepo.Application;
using OzdamarDepo.Infrastructure;
using OzdamarDepo.Infrastructure.Options;
using OzdamarDepo.WebAPI;
using OzdamarDepo.WebAPI.Controllers;
using OzdamarDepo.WebAPI.Modules;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Kestrel opsiyonel ayarlar
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1AndHttp2);
//    options.ListenAnyIP(5001, o => {
//        o.UseHttps();
//        o.Protocols = HttpProtocols.Http1AndHttp2;
//    });
//});

// Middleware ve servis kayıtları
builder.Services.AddResponseCompression(opt => opt.EnableForHttps = true);

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi();

builder.Services.AddControllers()
    .AddOData(opt =>
        opt.Select().Filter().Count().Expand().OrderBy()
            .SetMaxTop(null)
            .AddRouteComponents("odata", AppODataController.GetEdmModel()));



builder.Services.AddRateLimiter(x =>
    x.AddFixedWindowLimiter("fixed", cfg =>
    {
        cfg.QueueLimit = 100;
        cfg.Window = TimeSpan.FromSeconds(1);
        cfg.PermitLimit = 100;
        cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    }));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorization();



// Build uygulama
var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseStaticFiles(); // wwwroot içini servis eder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

app.UseRouting();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed"); //.RequireAuthorization();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapDefaultEndpoints();
app.RegisterRoutes();

// ✅ Kullanıcı oluşturmayı ApplicationStarted eventine bağla
app.Lifetime.ApplicationStarted.Register(() =>
{
    ExtensionsMiddleware.CreateFirstUser(app);
});

app.Run();
