using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.AdjustPatrimony;

public record AdjustPatrimonyCommand(string Code, decimal Amount) : IRequest<ErrorOr<Unit>>;