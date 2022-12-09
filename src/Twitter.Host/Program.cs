using TwitterAPIStatistics;
using TwitterHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new AppConfiguration
{
    ApiKey = builder.Configuration["ApiKey"],
    APIKeySecret = builder.Configuration["APIKeySecret"],
    BearerToken = builder.Configuration["BearerToken"],
    Url = builder.Configuration["Url"]
};

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddSingleton<ITwitterHandler>(new TwitterHandler.TwitterHandler(config.Url, config.BearerToken));
builder.Services.AddHostedService(services => services.GetService<ITwitterHandler>() as BackgroundService);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
