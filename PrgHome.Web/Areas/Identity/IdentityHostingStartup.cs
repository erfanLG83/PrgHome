using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrgHome.DataLayer;
using PrgHome.DataLayer.IdentityClasses;

[assembly: HostingStartup(typeof(PrgHome.Web.Areas.Identity.IdentityHostingStartup))]
namespace PrgHome.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddIdentity<AppUser, AppRole>()
                    .AddRoleManager<AppRoleManager>()
                    .AddEntityFrameworkStores<PrgHomeContext>()
                    .AddDefaultTokenProviders()
                    .AddErrorDescriber<AppIdentityErrorDescriber>();
                services.Configure<IdentityOptions>(op =>
                {
                    op.Password.RequireLowercase = false;
                    op.Password.RequiredLength = 8;
                    op.Password.RequireNonAlphanumeric = false;
                    op.Password.RequireUppercase = false;
                    op.User.RequireUniqueEmail = true;
                    op.SignIn.RequireConfirmedEmail = true;
                });
            });
        }
    }
}