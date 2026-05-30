using api.Services;

// Keep enough threads alive so the pool never has to inject on demand,
// which causes the 300ms stall every other request on Windows.
ThreadPool.SetMinThreads(50, 50);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Prevent connection timeouts from closing keep-alive connections
    // between rapid successive requests.
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllers();

builder.Services.AddSingleton<SortService>();
builder.Services.AddSingleton<FindService>();
builder.Services.AddSingleton<PatternService>();
builder.Services.AddSingleton<TreeService>();
builder.Services.AddSingleton<GraphService>();
builder.Services.AddSingleton<DynamicProgService>();
builder.Services.AddSingleton<BacktrackingService>();
builder.Services.AddSingleton<MiscService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
