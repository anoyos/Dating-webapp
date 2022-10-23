using james.ChatR;
using james.Helpers.General;
using james.Models.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james
{
    public class Startup
    {
        string AppBasepath = string.Empty;
        string AppRootBase = string.Empty;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            AppBasepath = _env.WebRootPath;
            AppRootBase = _env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<DBContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            services.AddSwaggerGen(c =>
            {

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line

            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddSignalR();
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbContextOptions<DBContext> dbOptions)
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
            //app.UseRequestResponseLogging();
            DBContext dbContext = new DBContext(dbOptions);
            dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
      
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service API V1");

            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/ChatR");
            });
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/Chat");
            //});

        }
    }
}
