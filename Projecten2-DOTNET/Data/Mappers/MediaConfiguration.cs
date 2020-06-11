using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_DOTNET.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class MediaConfiguration : IEntityTypeConfiguration<Media> {
		public void Configure(EntityTypeBuilder<Media> builder) {
			builder.ToTable("Media");

			builder.HasKey(m => m.MediaID);

			builder.Property(m => m.Type);

			builder.HasOne(m => m.Sessie).WithMany(m => m.Media);



		}
	}
}
