using ErrorOr;

namespace CaseItau.Application.Common.Errors;

public static class ApplicationErrors
{
    public static readonly Error FundNotFound = Error.NotFound(
        code: "Application.FundNotFound",
        description: "The requested fund was not found.");

    public static readonly Error FundTypeNotFound = Error.NotFound(
        code: "Application.FundTypeNotFound",
        description: "The requested fund type was not found.");

    public static readonly Error FundAlreadyExists = Error.Validation(
        code: "Application.FundAlreadyExists",
        description: "The requested fund already exists.");
}
