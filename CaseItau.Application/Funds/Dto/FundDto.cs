namespace CaseItau.Application.Funds.Dto;

public record FundDto(string Code, string Name, string Cnpj, long TypeId, string TypeName, decimal Patrimony);