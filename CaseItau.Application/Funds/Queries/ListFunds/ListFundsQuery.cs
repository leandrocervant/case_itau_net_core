using CaseItau.Application.Funds.Dto;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Queries.ListFunds;

public record ListFundsQuery : IRequest<ErrorOr<List<FundDto>>>;
