using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Persistence.Mappings;

public class FundTypeMap : IEntityTypeConfiguration<FundType>
{
    public void Configure(EntityTypeBuilder<FundType> builder)
    {
        builder.ToTable("FundTypes");

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(f => f.Name)
            .HasMaxLength(20)
            .IsRequired();
    }
}