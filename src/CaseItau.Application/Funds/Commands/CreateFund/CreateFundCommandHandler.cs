using CaseItau.Application.Common.Errors;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Repositories;
using CaseItau.Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.CreateFund;

internal class CreateFundCommandHandler(
    IFundTypeRepository fundTypeRepository,
    IFundRepository fundRepository,
    IUnitOfWork uow)
        : IRequestHandler<CreateFundCommand, ErrorOr<Fund>>
{
    private readonly IFundTypeRepository _fundTypeRepository = fundTypeRepository;
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Fund>> Handle(CreateFundCommand request, CancellationToken cancellationToken)
    {
        var fundType = await _fundTypeRepository.GetByIdAsync(request.TypeId);

        if (fundType is null)
        {
            return ApplicationErrors.FundTypeNotFound;
        }

        var existingFund = await _fundRepository.GetByCodeAsync(request.Code);

        if (existingFund is not null)
        {
            return ApplicationErrors.FundAlreadyExists;
        }

        var cnpj = new Cnpj(request.Cnpj);

        var fund = new Fund(
            request.Code,
            request.Name,
            cnpj,
            request.TypeId);

        await _fundRepository.AddAsync(fund);

        await _uow.CommitChangesAsync();

        return fund;
    }
}
