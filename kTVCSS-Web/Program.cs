using kTVCSS;
using kTVCSS.Controllers;
using kTVCSS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // add config
        builder.Configuration.AddJsonFile("config.json");

        builder.Services.AddMemoryCache();

        builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddAuthentication("kTVCSSv1")
               .AddCookie("kTVCSSv1", options =>
               {
                   options.AccessDeniedPath = "/NoAccess";
                   options.LoginPath = new PathString("/api/auth2");
                   options.Cookie.MaxAge = new TimeSpan(365, 1, 1, 1);
               });

        builder.Services.AddAuthorization(opts => {

        });

        var app = builder.Build();

        #region bind and show config

        Console.ForegroundColor = ConsoleColor.Green;

        app.Configuration.Bind(WebConfig.Instance);
        Console.WriteLine($"Database: {WebConfig.Instance.DbConnectionString}");

        Console.ForegroundColor = ConsoleColor.Gray;

        #endregion

        app.Environment.EnvironmentName = "Development";

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}