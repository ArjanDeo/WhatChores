using BlazorApp.Components;
using LazyCache;
using Microsoft.AspNetCore.Components;
using Models.BattleNet.Public;
using Models.RaiderIO.MythicPlus;
using Models.WhatChores;
using Models.WhatChores.API;
using Pathoschild.Http.Client;

namespace BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSingleton<WoWTokenPriceModel>();
            builder.Services.AddSingleton<List<RealmModel>>();
            builder.Services.AddSingleton<CharacterLookupModel>();
            builder.Services.AddSingleton<FluentClient>();
          //  builder.Services.AddSingleton<NavigationManager>();


			builder.Services.AddSingleton<RaiderIOMythicPlusAffixesModel>();

            builder.Services.AddScoped<IAppCache, CachingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
