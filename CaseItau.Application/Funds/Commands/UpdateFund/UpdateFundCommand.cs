using CaseItau.Domain.Entities;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.DeleteFund;

public record UpdateFundCommand(string Code, string Name, string Cnpj, long TypeId) : IRequest<ErrorOr<Fund>>;