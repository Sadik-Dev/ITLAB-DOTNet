using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class SessieGebruikerAanwezigConfiguration : IEntityTypeConfiguration<SessieGebruikerAanwezig> {
		public void Configure(EntityTypeBuilder<SessieGebruikerAanwezig> builder) {
			builder.HasKey(t => new { t.GebruikerId, t.SessieId });

			builder.HasOne(sg => sg.Sessie).WithMany(s => s.AanwezigeGebruikers).HasForeignKey(sg => sg.SessieId);
			builder.HasOne(s => s.Gebruiker).WithMany(s => s.AanwezigeSessies).HasForeignKey(s => s.GebruikerId);


		}
	}
}

