using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Serialization;
using Services;

namespace fcoupon
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			ConfigureAppSettings(env);
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.AddMvc()
			   .AddJsonOptions(opts =>
				{
					// Force Camel Case to JSON
					opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
			services.AddCors();
			services.AddLogging();

			var appSettings = Configuration.Get<AppSettings>();
			services.AddSingleton(appSettings);
			services.AddTransient<CouponService>();

			BsonClassMap.RegisterClassMap<FreeDeliveryCouponRule>();
			BsonClassMap.RegisterClassMap<MaxAmountThresholdDiscountCouponRule>();
			BsonClassMap.RegisterClassMap<FlatDiscountWithCapCouponRule>();
			BsonClassMap.RegisterClassMap<FlatDiscountWithoutCapCouponRule>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();

			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
			{
				builder.WithOrigins("http://localhost", "http://localhost:4000", "http://localhost:8080", "https://foodbeazt.in", "https://admin.foodbeazt.in")
				       .AllowAnyMethod()
				       .SetPreflightMaxAge(TimeSpan.FromDays(7))
					   .AllowAnyHeader();
			});

			app.UseExceptionHandler();
			//app.UseLoggerHandler();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		void ConfigureAppSettings(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
							.SetBasePath(env.ContentRootPath)
							.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
							.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
							.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; set; }
	}
}
