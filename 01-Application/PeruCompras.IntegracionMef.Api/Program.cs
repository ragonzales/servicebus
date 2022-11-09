using PeruCompras.IntegracionMef.Api;
using PeruCompras.IntegracionMef.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseEventHandler(builder.Configuration);
app.Run();