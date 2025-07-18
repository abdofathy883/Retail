using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.ModelConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName).IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnUpdate();

            builder.OwnsMany(p => p.RefreshTokens, rt =>
            {
                rt.WithOwner();
                rt.Property(r => r.Token)
                .IsRequired()
                .HasMaxLength(512);
                rt.Property(r => r.ExpiresOn)
                .IsRequired();
                rt.Property(r => r.CreateOn)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
