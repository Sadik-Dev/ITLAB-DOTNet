using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Projecten2_DOTNET.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projecten2_DOTNET.Models.Domein;
using Projecten2_DOTNET.Data.Repositories;
using Projecten2_DOTNET.Filter;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Projecten2_DOTNET {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddAuthorization(options => {
				options.AddPolicy("Verantwoordelijke", policy => policy.RequireClaim(ClaimTypes.Role, "verantwoordelijke"));
				options.AddPolicy("Gebruiker", policy => policy.RequireClaim(ClaimTypes.Role, "gebruiker"));
			});

			services.AddScoped<ISessieRepository, SessieRepository>();
			services.AddScoped<IGebruikerRepository, GebruikerRepository>();

			services.AddScoped<GebruikerFilter>();

			services.AddControllersWithViews()
				.AddRazorRuntimeCompilation();
			services.AddRazorPages();

			services.AddScoped<DataInitializer>();



		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataInitializer dataInitializer) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Overzicht}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});

			dataInitializer.InitializeData().Wait();
		}


	}
}
