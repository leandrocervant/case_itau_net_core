using CaseItau.Domain.Entities;
using CaseItau.Infrastructure.Persistence.ValueObjectMappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Persistence.Mappings;

public class FundMap : IEntityTypeConfiguration<Fund>
{
    public void Configure(EntityTypeBuilder<Fund> builder)
    {
        builder.ToTable("Funds");

        builder.HasIndex(f => f.Code)
            .IsUnique();

        builder.Property(f => f.Code)
             .HasMaxLength(20)
             .IsRequired();

        builder.Property(f => f.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.Patrimony)
            .HasColumnType("numeric");

        builder.OwnsOne(f => f.Cnpj, cnpj => cnpj.MapCnpj())
            .Navigation(f => f.Cnpj);

        builder.HasOne<FundType>()
            .WithMany()
            .HasForeignKey(f => f.TypeId)
            .IsRequired();
    }
}