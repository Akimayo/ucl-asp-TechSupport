using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TechSupport.Backend.Models;
using Microsoft.Net.Http.Headers;
using TechSupport.Backend.Hubs;

namespace TechSupport.Backend
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
            services.AddRazorPages();
            services.AddControllers();
            services.AddCors(opt => {
                opt.AddPolicy("ApiPolicy", builder => builder.AllowAnyOrigin().WithMethods("GET", "POST", "OPTIONS").WithHeaders(HeaderNames.ContentType, "multipart/form-data"));
            });
            services.AddSignalR();

            services.AddDbContext<IssuesContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("IssuesContext")));
            services.AddScoped<IReportingService, ReportingService>();
            services.AddScoped<IResolvingService, ResolvingService>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ReportsHub>("/ReportsHub");
            });
        }
    }
}
