using AlgorithmVisualizer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// TODO: What does this line do?
builder.Services.AddControllers();

// TODO: Why singleton? Why not Transient or Scoped?
builder.Services.AddSingleton<ArrayService>();
builder.Services.AddSingleton<StringService>();
builder.Services.AddSingleton<TreeService>();
builder.Services.AddSingleton<GraphService>();
builder.Services.AddSingleton<DynamicProgService>();
builder.Services.AddSingleton<BacktrackingService>();
builder.Services.AddSingleton<NrTheoryService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
