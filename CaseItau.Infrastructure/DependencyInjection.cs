using CaseItau.Application.Funds.Queries;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Repositories;
using CaseItau.Infrastructure.Persistence.Contexts;
using CaseItau.Infrastructure.Persistence.Queries;
using CaseItau.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaseItau.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<FundsDbContext>();
        services.AddDbContext<FundsDbContext>((serviceProvider, options) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            options.UseSqlite(config.GetConnectionString("Funds"));
        });

        //UoW
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<FundsDbContext>());

        //DomainEvents
        services.AddScoped<IDomainEventStore>(serviceProvider => serviceProvider.GetRequiredService<FundsDbContext>());

        //Data
        services.AddScoped<IFundRepository, FundRepository>();
        services.AddScoped<IFundTypeRepository, FundTypeRepository>();

        //Queries
        services.AddScoped<IFundQueryProvider, FundQueryProvider>();

        return services;
    }
}
