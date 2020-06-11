using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projecten2_DOTNET.Data.Mappers;
using Projecten2_DOTNET.Models.Domein;

namespace Projecten2_DOTNET.Data {
	public class ApplicationDbContext : IdentityDbContext {
		public DbSet<Gebruiker> Gebruikers { get; set; }
		public DbSet<Sessie> Sessies { get; set; }
		public DbSet<Lokaal> Lokalen { get; set; }
		public DbSet<FeedbackEntry> FeedbackEntries { get; set; }
		public DbSet<SessieGebruikerAanwezig> AanwezigeGebruikers { get; set; }
		public DbSet<SessieGebruikerIngeschreven> IngeschrevenGebruikers { get; set; }


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) {
		}

		protected override void OnModelCreating(ModelBuilder builder) {
			base.OnModelCreating(builder);
			builder.Entity<GebruikerState>();
			builder.Entity<NietActiefState>();
			builder.Entity<ActiefState>();
			builder.Entity<GeblokkeerdState>();

			builder.Entity<SessieState>();
			builder.Entity<RegistratieEnAanmeldenOpenState>();
			builder.Entity<RegistratieGeslotenAanmeldenOpenState>();
			builder.Entity<RegistratieOpenAanmeldenGeslotenState>();
			builder.Entity<AanmeldenEnRegistratieGeslotenState>();

			builder.Entity<Gebruiker>();
			builder.Entity<Verantwoordelijke>();
			builder.Entity<HoofdVerantwoordelijke>();

			builder.ApplyConfiguration(new SessieGebruikerAanwezigConfiguration());
			builder.ApplyConfiguration(new SessieGebruikerIngeschrevenConfiguration());
			builder.ApplyConfiguration(new SessieConfiguration());
			builder.ApplyConfiguration(new GebruikerConfiguration());
			builder.ApplyConfiguration(new MediaConfiguration());
			builder.ApplyConfiguration(new FeedbackEntryConfiguration());
			builder.ApplyConfiguration(new LokaalConfiguration());

		}
	}
}
