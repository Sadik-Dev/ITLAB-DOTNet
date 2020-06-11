using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_DOTNET.Data.Mappers {
	public class FeedbackEntryConfiguration : IEntityTypeConfiguration<FeedbackEntry> {
		public void Configure(EntityTypeBuilder<FeedbackEntry> builder) {
			builder.ToTable("FeedbackEntry");

			builder.HasKey(f => f.Id);

			builder.Property(f => f.Score).IsRequired();
			builder.Property(f => f.Inhoud).HasMaxLength(255).IsRequired();

			builder.HasOne(p => p.Auteur);
		}
	}
}
