using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrgHome.DataLayer;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.IdentityServices;
using PrgHome.DataLayer.UnitOfWork;
using PrgHome.Web.Classes;
using PrgHome.Web.Models;
using PrgHome.Web.Services;

namespace PrgHome.Web
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

            services.AddControllersWithViews();
            //services.AddRazorPages();
            services.Configure<CookiePolicyOptions>(op =>
            {
                op.CheckConsentNeeded = context=>true;
                op.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
            services.AddSession(op =>
            {
                op.IdleTimeout = TimeSpan.FromMinutes(20);
                op.Cookie.HttpOnly = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
            });
            services.AddDbContext<PrgHomeContext>(options=>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PrgHome"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileWorker, FileWorker>();
            services.AddScoped<AppIdentityErrorDescriber>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IAppRoleManager, AppRoleManager>();
            services.AddScoped<IAppUserManager,AppUserManager>();
            services.AddScoped<ConvertDate>();
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] {
                    UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic 
                }));
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
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapAreaControllerRoute(
                    name:"admin",
                    areaName:"admin",
                    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapRazorPages();
            });
        }
    }
}
