using CaseItau.Application.Common.Errors;
using CaseItau.Application.Funds.Dto;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Queries.GetFund;

internal class GetFundQueryHandler(IFundQueryProvider queries) : IRequestHandler<GetFundQuery, ErrorOr<FundDto>>
{
    private readonly IFundQueryProvider _queries = queries;

    public async Task<ErrorOr<FundDto>> Handle(GetFundQuery request, CancellationToken cancellationToken)
    {
        var fund = await _queries.GetFundAsync(request.Code);

        if (fund is null)
        {
            return ApplicationErrors.FundNotFound;
        }

        return fund;
    }
}
