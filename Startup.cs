using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RubyMine.DbContexts;
using RubyMine.DIP;
using RubyMine.DIP.Impl;
using RubyMine.Models;
using System;

namespace RubyMine {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddSession(a => a.IdleTimeout = TimeSpan.FromMinutes(30));

            var serverVersion = new MySqlServerVersion(new System.Version(5, 7, 32));
            services.AddDbContext<RubyRemineDbContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MySQL_remote"), serverVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()); ;
            services.AddScoped<IDIPSetting, DIPSetting>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });
        }
    }
}