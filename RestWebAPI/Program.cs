using RestWebAPI.Extensions;
using RestWebAPI.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IMockApiService, MockApiService>();


var app = builder.Build();

app.RouteMockAPIEndpoints();

app.Run();

