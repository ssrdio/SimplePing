using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Transactions;
using SimpleLsSample.Models;
using SimpleLsSample.Interfaces;
using SimpleLsSample.DAL.DAO;
using Microsoft.OpenApi.Models;
using NLog;
using SimpleLsSample.Services;

namespace SimpleLsSample
{
    public class Startup
    {
        internal static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            Environment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PingerDBContext>();
            services.AddTransient<IResultDao, ResultDao>();
            services.AddTransient<IResultBl, ResultBl>();
            services.AddTransient<LoggingService>();
            services.AddControllers();
            
        
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(new GlobalExceptionFilter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SimpleSamle", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                PingerDBContext context = scope.ServiceProvider.GetRequiredService<PingerDBContext>();
                context.Database.EnsureCreated();
                if (!context.FriendlyMessages.Any())
                {
                    // seed data
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Nepravilen format datuma"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Nepričakovana izjema"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Neustrezna uporaba niti (threading)"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Napaka pri uporabi zunanjih virov"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Nepravilen dostop"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Potrebna avtorizacija"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Potrebna avtentikacija"
                    });
                    context.FriendlyMessages.Add(new ErrorMessages
                    {
                        Language = "SI",
                        Message = "Napaka med stolom in monitorjem"
                    });
                    context.SaveChanges();
                }
            }

                app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "second",
                   pattern: "/api/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Result}/{action=Find}/{id?}");
                
            });

        }
    }
}
