using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Inventory;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using _01_LampshadeQuery.Contracts.Slide;
using _01_LampshadeQuery.Query;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Infrastructure.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authorization;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Cart;
using ShopManagement.Infrastructure.Configuration;
using ICookieManager = _01_LampshadeQuery.ICookieManager;

namespace Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddHttpContextAccessor();
            string? databaseType = builder.Configuration["DatabaseType"];
            string? connectionString = builder.Configuration.GetConnectionString("LampShadeDB");
            DiscountManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);
            ShopManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);
            InventoryManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);
            BlogManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);
            CommentManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);
            AccountManagementBootstrapper.Configure(builder.Services, connectionString, databaseType);

            builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddTransient<IFileUploader, FileUploader>();
            builder.Services.AddTransient<IAuthHelper, AuthHelper>();
            builder.Services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
            builder.Services.AddTransient<ICookieManager, CookieManager>();
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
            builder.Services.AddSingleton<ISmsService, SmsService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddTransient<ICartCalculatorService, CartCalculatorService>();
            builder.Services.AddSingleton<ICartService, CartService>();
            builder.Services.AddTransient<ISlideQuery, SlideQuery>();
            builder.Services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();
            builder.Services.AddTransient<IProductQuery, ProductQuery>();
            builder.Services.AddTransient<ICartQuery, CartQuery>();
            builder.Services.AddTransient<IInventoryQuery, InventoryQuery>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Host", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500") // ?? your Angular dev port
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("Host");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}