
using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Query;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Infrastructure.Configuration;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authorization;
using ServiceHost;
using ShopManagement.Infrastructure.Configuration;

namespace ShopPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor();
            // Add services to the container.
            DiscountManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            ShopManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            InventoryManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            BlogManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            CommentManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            AccountManagementBootstrapper.Configure(builder.Services,
                builder.Configuration.GetConnectionString("LampShadeDB"));
            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddTransient<IFileUploader, FileUploader>();
            builder.Services.AddTransient<IAuthHelper, AuthHelper>();
            builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
            builder.Services.AddTransient<ICookieManager, CookieManager>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
            builder.Services.AddSingleton<ISmsService, SmsService>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontEnd", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // 👈 your Angular dev port
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("FrontEnd");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
