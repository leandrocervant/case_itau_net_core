using CaseItau.API;
using CaseItau.Application;
using CaseItau.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure();
};

var app = builder.Build();
{
    app.UseExceptionHandler();
    app.UseInfrastructure();
    app.UseHealthChecks("/health");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

public partial class Program
{
}
