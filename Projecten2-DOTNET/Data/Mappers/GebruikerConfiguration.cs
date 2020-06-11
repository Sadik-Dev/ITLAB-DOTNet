using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class GebruikerConfiguration : IEntityTypeConfiguration<Gebruiker> {
		public void Configure(EntityTypeBuilder<Gebruiker> builder) {
			builder.ToTable("Gebruiker");

			builder.HasKey(g => g.Gebruikersnaam);

			builder.Property(g => g.Barcode).IsRequired();
			builder.Property(g => g.Naam).IsRequired();

			builder.HasMany(b => b.IngeschrevenSessies).WithOne(p => p.Gebruiker);
			builder.HasMany(b => b.AanwezigeSessies).WithOne(p => p.Gebruiker);
			builder.HasMany(b => b.GeorganiseerdeSessies).WithOne(p => p.Verantwoordelijke);

			builder.HasOne(b => b.CurrentState).WithOne(p => p.Context).HasForeignKey<GebruikerState>(l => l.Id);
		}
	}
}
