using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RubyMine.DbContexts;
using RubyMine.DIP;
using RubyMine.DIP.Impl;
using RubyMine.Jwt;
using RubyMine.Models;
using System;
using System.Text;

namespace RubyMine {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddControllers();
            services.AddSession(a => a.IdleTimeout = TimeSpan.FromMinutes(30));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Rubymine for Navicat",
                    Version = "v1",
                    Description = "四川劳吉克信息技术有限公司"
                });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer()
                .AddCookie(x => {
                    x.LoginPath = "/";
                    x.LogoutPath = "/Logout";
                    x.AccessDeniedPath = "/AccessDenied";
                    x.ReturnUrlParameter = "ReturnUrl";
                });

            var serverVersion = new MySqlServerVersion(new System.Version(5, 7, 32));
            services.AddDbContext<RubyRemineDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MySQL_local"), serverVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                );
            services.AddScoped<IGlobalSetting, GlobalSetting>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseStaticFiles();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger(c => {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rubymine for Navicat v1"));

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}