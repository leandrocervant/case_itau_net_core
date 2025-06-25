using CaseItau.Application.Common.Errors;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Repositories;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.DeleteFund;

internal class DeleteFundCommandHandler(
    IFundRepository fundRepository,
    IUnitOfWork uow)
        : IRequestHandler<DeleteFundCommand, ErrorOr<Unit>>
{
    private readonly IFundRepository _fundRepository = fundRepository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<Unit>> Handle(DeleteFundCommand request, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetByCodeAsync(request.Code);

        if (fund is null)
        {
            return ApplicationErrors.FundNotFound;
        }

        await _fundRepository.RemoveAsync(fund);

        await _uow.CommitChangesAsync();

        return Unit.Value;
    }
}
