using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class SessieGebruikerIngeschrevenConfiguration : IEntityTypeConfiguration<SessieGebruikerIngeschreven> {
		public void Configure(EntityTypeBuilder<SessieGebruikerIngeschreven> builder) {
			builder.HasKey(t => new { t.GebruikerId, t.SessieId });

			builder.HasOne(s => s.Sessie).WithMany(s => s.IngeschrevenGebruikers).HasForeignKey(s => s.SessieId);
			builder.HasOne(s => s.Gebruiker).WithMany(s => s.IngeschrevenSessies).HasForeignKey(s => s.GebruikerId);

		}
	}
}
