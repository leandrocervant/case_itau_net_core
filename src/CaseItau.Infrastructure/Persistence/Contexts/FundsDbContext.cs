using CaseItau.Domain.Common;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.Infrastructure.Persistence.Contexts
{
    public class FundsDbContext(DbContextOptions<FundsDbContext> options) : DbContext(options), IUnitOfWork, IDomainEventStore
    {
        public DbSet<Fund> Funds { get; set; } = null!;

        public DbSet<FundType> FundTypes { get; set; } = null!;

        public Task<IEnumerable<IDomainEvent>> GetDomainEventsAsync()
        {
            // get hold of all the domain events
            var domainEvents = ChangeTracker.Entries<Entity>()
                .Select(entry => entry.Entity.PopEvents())
                .SelectMany(events => events);

            return Task.FromResult(domainEvents);
        }

        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FundsDbContext).Assembly);

            var properties = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
                {
                    // primary key configuration
                    modelBuilder.Entity(entityType.ClrType)
                        .HasKey(nameof(Entity.Id));

                    // configure Id property
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(Entity.Id))
                        .IsRequired()
                        .ValueGeneratedOnAdd();
                }
            }

            // configure some common property types
            var stringProperties = properties.Where(p => p.ClrType == typeof(string));
            foreach (var property in stringProperties)
            {
                var maxLength = property.GetMaxLength() ?? 50;

                property.SetColumnType($"varchar({maxLength})");
            }

            var booleanProperties = properties
                .Where(p => p.ClrType == typeof(bool) ||
                            p.ClrType == typeof(bool?));

            foreach (var property in booleanProperties)
            {
                property.SetColumnType("bit");
                property.IsNullable = false;
            }

            var dateTimeProperties = properties.Where(p => p.ClrType == typeof(DateTime));

            foreach (var property in dateTimeProperties)
            {
                property.SetColumnType("datetime");
            }

            var enumProperties = properties.Where(p => p.ClrType == typeof(Enum));

            foreach (var property in enumProperties)
            {
                property.SetColumnType("smallint");
            }

            var amountProperties = properties
                .Where(p => p.ClrType == typeof(decimal) ||
                            p.ClrType == typeof(decimal?));

            foreach (var property in amountProperties)
            {
                property.SetColumnType("numeric");
            }

            SeedContext(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedContext(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FundType>()
                .HasData(
                    new { Id = 1L, Name = "RENDA FIXA", IsDeleted = false },
                    new { Id = 2L, Name = "ACOES", IsDeleted = false },
                    new { Id = 3L, Name = "MULTI MERCADO", IsDeleted = false }
                );

            modelBuilder.Entity<Fund>().OwnsOne(c => c.Cnpj)
                .HasData(
                     new { FundId = 1L, Value = "11222333444455" },
                     new { FundId = 2L, Value = "12222333444455" },
                     new { FundId = 3L, Value = "13222333444455" },
                     new { FundId = 4L, Value = "14222333444455" },
                     new { FundId = 5L, Value = "11222333444777" }
                );

            modelBuilder.Entity<Fund>()
                .HasData(
                    new { Id = 1L, Code = "ITAURF123", Name = "ITAU JUROS RF +", TypeId = 1L, Patrimony = 5498731.54m, IsDeleted = false },
                    new { Id = 2L, Code = "ITAUMM999", Name = "ITAU TREND MM", TypeId = 3L, Patrimony = 5m, IsDeleted = false },
                    new { Id = 3L, Code = "ITAURF321", Name = "ITAU LONGO PRAZO RF +", TypeId = 1L, Patrimony = 7875421.58m, IsDeleted = false },
                    new { Id = 4L, Code = "ITAUAC546", Name = "ITAU ACOES DIVIDENDO", TypeId = 2L, Patrimony = 66421254.83m, IsDeleted = false },
                    new { Id = 5L, Code = "ITAURF555", Name = "ITAU JUROS RF +", TypeId = 1L, Patrimony = 0m, IsDeleted = false }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }
    }
}
