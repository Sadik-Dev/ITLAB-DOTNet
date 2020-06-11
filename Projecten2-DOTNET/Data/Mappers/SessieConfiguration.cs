using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class SessieConfiguration : IEntityTypeConfiguration<Sessie> {
		public void Configure(EntityTypeBuilder<Sessie> builder) {
			builder.ToTable("Sessie");



			builder.HasKey(s => s.Id);



			builder.Property(s => s.Titel).IsRequired();
			builder.Property(s => s.GastSpreker).IsRequired();



			builder.HasMany(b => b.IngeschrevenGebruikers).WithOne(p => p.Sessie);
			builder.HasMany(b => b.AanwezigeGebruikers).WithOne(p => p.Sessie);

			builder.HasOne(s => s.Verantwoordelijke).WithMany(v => v.GeorganiseerdeSessies);

			builder.HasOne(s => s.Lokaal);

			builder.HasOne(b => b.CurrentState).WithOne(b => b.Context).HasForeignKey<SessieState>(b => b.Id);

			builder.HasMany(s => s.FeedbackEntries);

			builder.HasMany(b => b.Media).WithOne(c => c.Sessie);
		}
	}
}
