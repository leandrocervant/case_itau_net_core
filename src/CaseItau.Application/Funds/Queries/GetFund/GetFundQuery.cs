using CaseItau.Application.Funds.Dto;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Queries.GetFund;

public record GetFundQuery(string Code) : IRequest<ErrorOr<FundDto>>;
