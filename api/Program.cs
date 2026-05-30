using api.Services;

var builder = WebApplication.CreateBuilder(args);

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
