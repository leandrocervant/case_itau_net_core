using CaseItau.Application.Common.Errors;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Repositories;
using CaseItau.Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.DeleteFund;

public class UpdateFundCommandHandler(
    IFundTypeRepository fundTypeRepository,
    IFundRepository fundRepository,
    IUnitOfWork uow)
        : IRequestHandler<UpdateFundCommand, ErrorOr<Fund>>
{
    private readonly IFundTypeRepository _fundTypeRepository = fundTypeRepository;
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Fund>> Handle(UpdateFundCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCodeAsync(request.Code);

        if (fund is null)
        {
            return ApplicationErrors.FundNotFound;
        }

        var fundType = await _fundTypeRepository.GetByIdAsync(request.TypeId);

        if (fundType is null)
        {
            return ApplicationErrors.FundTypeNotFound;
        }

        fund.Name = request.Name;
        fund.Cnpj = new Cnpj(request.Cnpj);
        fund.TypeId = request.TypeId;

        await _uow.CommitChangesAsync();

        return fund;
    }
}
