using CaseItau.Application.Funds.Commands.AdjustPatrimony;
using CaseItau.Application.Funds.Commands.CreateFund;
using CaseItau.Application.Funds.Commands.DeleteFund;
using CaseItau.Application.Funds.Queries.GetFund;
using CaseItau.Application.Funds.Queries.ListFunds;
using CaseItau.Contracts.Funds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CaseItau.API.Controllers;

[Route("api/v1/[controller]")]
public class FundsController(ISender _mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateFund(CreateFundRequest request)
    {
        var command = new CreateFundCommand(
            request.Code,
            request.Name,
            request.Cnpj,
            request.TypeId);

        var createFundResult = await _mediator.Send(command);

        return createFundResult.Match(
            fund => CreatedAtAction(
                nameof(GetFund),
                new { fund.Code },
                new CreateFundResponse(fund.Code)),
            Problem);
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateFund(string code, UpdateFundRequest request)
    {
        var command = new UpdateFundCommand(
            code,
            request.Name,
            request.Cnpj,
            request.TypeId);

        var updateFundResult = await _mediator.Send(command);

        return updateFundResult.Match(
            fund => Ok(new UpdateFundResponse(
                fund.Name,
                fund.Cnpj.Value,
                fund.TypeId)),
            Problem);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteFund(string code)
    {
        var command = new DeleteFundCommand(code);

        var deleteFundResult = await _mediator.Send(command);

        return deleteFundResult.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> ListFunds()
    {
        var query = new ListFundsQuery();

        var listFundsResult = await _mediator.Send(query);

        return listFundsResult.Match(
            funds => Ok(funds.ConvertAll(fund => new FundResponse(
                fund.Code,
                fund.Name,
                fund.Cnpj,
                fund.TypeId,
                fund.TypeName,
                fund.Patrimony))),
            Problem);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetFund(string code)
    {
        var query = new GetFundQuery(code);

        var getFundResult = await _mediator.Send(query);

        return getFundResult.Match(
            fund => Ok(new FundResponse(
                fund.Code,
                fund.Name,
                fund.Cnpj,
                fund.TypeId,
                fund.TypeName,
                fund.Patrimony)),
            Problem);
    }

    [HttpPatch("{code}/patrimony")]
    public async Task<IActionResult> AdjustPatrimony(string code, [FromBody] AdjustPatrimonyRequest request)
    {
        var command = new AdjustPatrimonyCommand(code, request.Patrimony);

        var updatePatrimonyResult = await _mediator.Send(command);

        return updatePatrimonyResult.Match(
            _ => NoContent(),
            Problem);
    }
}
