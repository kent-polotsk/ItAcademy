
using DAL_CQS_.Commands;
using EFDatabase;
using GNA.Services.Abstractions;
using GNA.Services.Implementations;
using Mappers.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OtpNet;
using Serilog;
using System.Reflection;

namespace GNAggregator.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();


            builder.Services.AddSerilog();
            builder.Host.UseSerilog(Log.Logger);

            builder.Services.AddDbContext<GNAggregatorContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            //builder.Services.Configure<EmailSettings>(
            //        builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<ICodeGeneratorService, CodeGeneratorService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITotpCodeService, TotpCodeService>();

            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddScoped<IRssService, RssService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddMediatR(sc => sc.RegisterServicesFromAssembly(typeof(AddArticlesCommand).Assembly));
            builder.Services.AddTransient<ArticleMapper>();
            builder.Services.AddTransient<UserMapper>();
            builder.Services.AddTransient<SourceMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt=>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "WebApp API",
                    Version = "v1",
                    Description = "WebApp API"
                });

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); 
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
