using EFDatabase;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using GNA.Services.Samples;
using Microsoft.EntityFrameworkCore;
namespace WebAppGNAggregator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GNAggregatorContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IAuthorsService, AuthorsService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();

            builder.Services.AddTransient<ITransientService, TransietnService>();
            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();
            
            builder.Services.AddScoped<ITestService, TestService>();

            

         
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
