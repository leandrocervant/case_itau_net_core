using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.DeleteFund;

public record DeleteFundCommand(string Code) : IRequest<ErrorOr<Unit>>;