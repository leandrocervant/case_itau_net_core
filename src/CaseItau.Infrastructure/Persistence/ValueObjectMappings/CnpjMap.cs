using CaseItau.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseItau.Infrastructure.Persistence.ValueObjectMappings
{
    public static class CnpjMap
    {
        public static void MapCnpj<TEntity>(this OwnedNavigationBuilder<TEntity, Cnpj> navigationBuilder)
           where TEntity : class
        {
            navigationBuilder.WithOwner();

            navigationBuilder.Property(x => x.Value)
                .HasMaxLength(14)
                .HasColumnName("Cnpj")
                .IsRequired();
        }
    }
}
