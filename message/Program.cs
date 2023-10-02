using message.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Redis
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = "185.234.114.133:6379,abortConnect=False";
});


builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHealthChecks("/");

app.Run();
