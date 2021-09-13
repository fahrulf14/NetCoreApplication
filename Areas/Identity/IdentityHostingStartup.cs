using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUNA.Data;

[assembly: HostingStartup(typeof(NUNA.Areas.Identity.IdentityHostingStartup))]
namespace NUNA.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NUNAIdentityContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("ApplicationBase")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<NUNAIdentityContext>();
            });
        }
    }
}