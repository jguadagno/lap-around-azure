using Azure.Storage.Blobs.Models;
using Contacts.WebUi.Models;
using Contacts.WebUi.Services;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Contacts.WebUi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new Settings();
            Configuration.Bind("Settings", settings);
            services.AddSingleton(settings);
            
            // DEMO: AppInsights
            services.AddApplicationInsightsTelemetry();
            
            services.AddSingleton(provider =>
            {
                var blobs = new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName);
                var blob = blobs.BlobContainerClient.SetAccessPolicy(accessType: PublicAccessType.Blob);
                return blobs;
            });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = 
                    Configuration.GetConnectionString("ContactsDatabaseSqlServer");
                options.SchemaName = "dbo";
                options.TableName = "Cache";
            });
            
            services.AddControllersWithViews();
            
            services.AddContactService();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}