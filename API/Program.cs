using DataAccess;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models.WhatChores.API;
using Pathoschild.Http.Client;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddSingleton<CachingService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "What Chores? API",
        Description = "An API to get information about World of Warcraft\n\nThe base uri is https://localhost:7031/\n\nThe data is from my web app, <a href=\"https://localhost:44368\">What Chores</a>.",       
    });
    options.CustomSchemaIds(type => $"{type.Name}_{Guid.NewGuid()}");
});



if(builder.Environment.IsDevelopment())
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
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
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
app.UseCors("CorsPolicy");

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
