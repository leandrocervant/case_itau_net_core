using CaseItau.Application.Funds.Dto;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Queries.ListFunds;

internal class ListFundsQueryHandler(IFundQueryProvider queries) : IRequestHandler<ListFundsQuery, ErrorOr<List<FundDto>>>
{
    private readonly IFundQueryProvider _queries = queries;

    public async Task<ErrorOr<List<FundDto>>> Handle(ListFundsQuery request, CancellationToken cancellationToken)
    {
        var funds = await _queries.ListFundsAsync();

        return funds;
    }
}