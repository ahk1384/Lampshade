using DiscountManagement.Infrastructure.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using ShopManagement.Infrastructure.Configuration;

namespace ServiceHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            DiscountManagementBootstrapper.Configure(builder.Services, builder.Configuration.GetConnectionString("LampShadeDB"));
            ShopManagementBootstrapper.Configure(builder.Services, builder.Configuration.GetConnectionString("LampShadeDB"));
            InventoryManagementBootstrapper.Configure(builder.Services, builder.Configuration.GetConnectionString("LampShadeDB"));
            builder.Services.AddRazorPages();

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

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
