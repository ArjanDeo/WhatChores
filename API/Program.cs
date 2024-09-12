using DataAccess;
using HtmlAgilityPack;
using LazyCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models.WhatChores.API;
using Pathoschild.Http.Client;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddSingleton<CachingService>();
builder.Services.AddSingleton<HtmlDocument>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "What Chores? API",
            Description = "An API to get information about World of Warcraft\n\n" +
        "The base uri is https://localhost:7031/" +
        "\n\nThe data is for my web app, <a href=\"https://localhost:5173\">What Chores</a>.",
        });
    } else
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "What Chores? API",
            Description = "An API to get information about World of Warcraft\n\n" +
       "The base uri is https://whatchoresapi.azurewebsites.net" +
       "\n\nThe data is for my web app, <a href=\"https://whatchores.furyshiftz.com\">What Chores</a>.",
        });
    }
    
    options.CustomSchemaIds(type => $"{type.Name}_{Guid.NewGuid()}");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-bn6n7i4zrwxk0s0y.us.auth0.com/";
    options.Audience = "https://whatchores.furyshiftz.com";
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<WhatChoresDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
    builder.Services.Configure<SettingsModel>(builder.Configuration.GetSection("DevSettings"));

}
else
{
    builder.Services.AddDbContext<WhatChoresDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProdConnection")));
    builder.Services.Configure<SettingsModel>(builder.Configuration.GetSection("ProdSettings"));

}
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://127.0.0.1", "http://localhost:5173", "http://localhost")
            .AllowAnyHeader());
    options.AddPolicy("ProdPolicy",
        builder => builder
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("https://whatchores.furyshiftz.com")
        .AllowAnyHeader());
});

builder.Services.AddSingleton<FluentClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
    });
//}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevPolicy");
} else
{
    app.UseCors("ProdPolicy");
}
app.UseAuthentication();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }