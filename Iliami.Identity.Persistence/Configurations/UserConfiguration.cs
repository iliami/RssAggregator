using Iliami.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iliami.Identity.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).HasMaxLength(32);

        builder.Property(x => x.Password).HasMaxLength(128);

        builder.Property(x => x.Role).HasMaxLength(16);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}