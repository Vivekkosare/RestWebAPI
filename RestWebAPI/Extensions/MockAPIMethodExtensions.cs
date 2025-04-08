using Microsoft.AspNetCore.Mvc;
using RestWebAPI.Entities;
using RestWebAPI.Services;

namespace RestWebAPI.Extensions
{
    public static class MockAPIMethodExtensions
    {
        public static IEndpointRouteBuilder RouteMockAPIEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/mockdata");
            builder.MapGet("/", async (IMockApiService apiService, [FromQuery] int? page,
                [FromQuery] int? pageSize, [FromQuery] string? name) =>
            {
                var result = await apiService.GetPhonesAsync(page, pageSize, name);
                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }
                return result.Error switch
                {
                    "NotFound" => Results.NotFound(result.Error),
                    "BadRequest" => Results.BadRequest(result.Error),
                    _ => Results.Problem(result.Error)
                };
            });

            return builder;
        }
    }
}
