using DAL_CQS_.Commands;
using EFDatabase;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using Mappers.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace WebAppGNAggregator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            //builder.Environment.EnvironmentName = "Production";

            builder.Services.AddControllersWithViews();

            builder.Services.AddSerilog();

            builder.Services.AddDbContext<GNAggregatorContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IRssService, RssService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddMediatR(sc => sc.RegisterServicesFromAssembly(typeof(AddArticlesCommand).Assembly));
            builder.Services.AddTransient<ArticleMapper>();
            builder.Services.AddTransient<UserMapper>();


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
                    opt.LoginPath = "/account/login";
                    opt.AccessDeniedPath = "/account/accessdenied";
                    opt.LogoutPath = "/account/logout";
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            //}

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");


            app.UseHttpsRedirection();
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
}
