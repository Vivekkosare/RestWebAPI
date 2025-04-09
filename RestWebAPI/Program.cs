using FluentValidation;
using FluentValidation.AspNetCore;
using RestWebAPI.Extensions;
using RestWebAPI.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IMockApiService, MockApiService>();

//Add fluent validation
builder.Services
	.AddFluentValidationAutoValidation()
	.AddValidatorsFromAssemblyContaining<RestWebAPI.Validators.PhoneValidator>()
	.AddValidatorsFromAssemblyContaining<RestWebAPI.Validators.PhoneInputValidator>();

var app = builder.Build();

app.RouteMockAPIEndpoints();

app.Run();

