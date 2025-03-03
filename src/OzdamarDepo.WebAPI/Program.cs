using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using OzdamarDepo.Application;
using OzdamarDepo.Infrastructure;
using OzdamarDepo.WebAPI;
using OzdamarDepo.WebAPI.Controllers;
using OzdamarDepo.WebAPI.Modules;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();

builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        //.EnableQueryFeatures()
        .AddRouteComponents("odata", AppODataController.GetEdmModel())
//
);


builder.Services.AddOpenApi();
builder.Services.AddRateLimiter(x =>
x.AddFixedWindowLimiter("fixed", cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();


builder.AddServiceDefaults();
var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapDefaultEndpoints();
app.MapControllers().RequireRateLimiting("fixed");
app.RegisterRoutes();

app.UseExceptionHandler();


app.UseCors(x => x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().SetIsOriginAllowed(t => true));
app.Run();
