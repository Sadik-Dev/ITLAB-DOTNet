using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class LokaalConfiguration : IEntityTypeConfiguration<Lokaal> {
		public void Configure(EntityTypeBuilder<Lokaal> builder) {
			builder.HasKey(l => l.Id);

			builder.Property(l => l.Lokaalcode).IsRequired();
		}
	}
}
