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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Cart;
using ShopManagement.Infrastructure.Configuration;
using ICookieManager = _01_LampshadeQuery.ICookieManager;

namespace ServiceHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpContextAccessor();
        // Add services to the container.
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
        builder.Services.AddTransient<ISlideQuery, SlideQuery>();
        builder.Services.AddTransient<IProductCategoryQuery, ProductCategoryQuery>();
        builder.Services.AddTransient<IProductQuery, ProductQuery>();
        builder.Services.AddTransient<ICartQuery, CartQuery>();
        builder.Services.AddTransient<IInventoryQuery, InventoryQuery>();
        builder.Services.AddTransient<ICartCalculatorService, CartCalculatorService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.LoginPath = new PathString("/Login");
                o.LogoutPath = new PathString("/Logout");
                o.AccessDeniedPath = new PathString("/AccessDenied");
            });
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
            builder
                .WithOrigins("https://localhost:5002")
                .AllowAnyHeader()
                .AllowAnyMethod()));

        builder.Services
            .AddRazorPages()
            .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
            .AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/", "Admin");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Shop/Product", "product");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Shop/ProductCategories", "productCategory");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Shop/ProductPictures", "productPictures");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Shop/Slides", "slide");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Shop/Orders", "orders");

                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Inventory", "inventory");

                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Discounts", "discount");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Discounts/CustomerDiscount",
                    "customerDiscount");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Discounts/ColleagueDiscount",
                    "colleagueDiscount");

                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Comments", "comment");

                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Blog", "blog");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Blog/Articles", "article");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Blog/ArticleCategories", "articleCategories");

                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Accounts", "account");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Accounts/Account", "accountManagement");
                options.Conventions.AuthorizeAreaFolder("Adminstrator", "/Accounts/Role", "role");
            });

        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseAuthentication();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCookiePolicy();
        app.UseRouting();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapDefaultControllerRoute();
        app.Run();
    }
}