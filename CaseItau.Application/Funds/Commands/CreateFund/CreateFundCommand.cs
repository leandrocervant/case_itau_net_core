using CaseItau.Domain.Entities;
using ErrorOr;
using MediatR;

namespace CaseItau.Application.Funds.Commands.CreateFund;

public record CreateFundCommand(
    string Code,
    string Name,
    string Cnpj,
    long TypeId) : IRequest<ErrorOr<Fund>>;
