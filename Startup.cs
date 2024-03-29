using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NUNA.Models.BaseApplicationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUNA.Services;
using NUNA.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace NUNA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            configuration.GetSection("AppSettings").Bind(AppSettingHelper.GetValue);
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("ApplicationBase");
            services.AddDbContext<BaseApplicationContext>(options => options.UseNpgsql(connection));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IWebHostEnvironment>();
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time   
            });

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson();

            services.AddHttpContextAccessor();
            services.AddSingleton<SessionHandler>();
            services.AddTransient<UserService>();

            services.Configure<RazorViewEngineOptions>(opt =>
            {
                opt.ViewLocationExpanders.Add(new ComponentViewLocationExpander());
            });
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
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();

            SessionService.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                     name: "defaultWithoutAction",
                     pattern: "{controller}/{id?}",
                     defaults: new { action = "Index" },
                     constraints: new { id = @"\d+" }
                   );

                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                     name: "defaultAdmin",
                     pattern: "Admin/{controller}/{id?}",
                     defaults: new { action = "Index" },
                     constraints: new { id = @"\d+" }
                   );

                endpoints.MapRazorPages();
            });
        }
    }
}
