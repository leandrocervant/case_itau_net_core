using CaseItau.Application.Common.Errors;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.AdjustPatrimony;

internal class AdjustPatrimonyCommandHandler(
    IFundRepository fundRepository,
    IUnitOfWork uow)
        : IRequestHandler<AdjustPatrimonyCommand, ErrorOr<Unit>>
{
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Unit>> Handle(AdjustPatrimonyCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCodeAsync(request.Code);

        if (fund is null)
        {
            return ApplicationErrors.FundNotFound;
        }

        await _fundRepository.AdjustPatrimonyAsync(fund.Code, request.Amount);

        await _uow.CommitChangesAsync();

        return Unit.Value;
    }
}
