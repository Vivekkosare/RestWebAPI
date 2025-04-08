using Microsoft.AspNetCore.Mvc;
using RestWebAPI.Entities;
using RestWebAPI.Services;
using System.Net;

namespace RestWebAPI.Extensions
{
    public static class MockAPIMethodExtensions
    {
        public static IEndpointRouteBuilder RouteMockAPIEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/mockdata");

            /// <summary>
            /// Get all phones
            /// </summary>
            /// <param name="apiService"></param>
            /// <param name="page"></param>
            /// <param name="pageSize"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <response code="200">Phones retrieved successfully</response>
            builder.MapGet("/", async (IMockApiService apiService, [FromQuery] int? page,
                [FromQuery] int? pageSize, [FromQuery] string? name) =>
            {
                var result = await apiService.GetPhonesAsync(page, pageSize, name);
                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }
                return result.HttpStatusCode switch
                {
                    HttpStatusCode.BadRequest => Results.BadRequest(result.Error),
                    HttpStatusCode.NotFound => Results.NotFound(result.Error),
                    _ => Results.Problem(result.Error)
                };
            });

            /// <summary>
            /// Add a new phone
            /// </summary>
            /// <param name="apiService"></param>
            /// <param name="phone"></param>
            builder.MapPost("/", async (IMockApiService apiService, Phone phone) =>
            {
                var result = await apiService.AddPhoneAsync(phone);
                if (result.IsSuccess)
                {
                    return Results.Created($"/mockdata/{result.Value.Id}", result.Value);
                }
                return result.HttpStatusCode switch
                {
                    HttpStatusCode.BadRequest => Results.BadRequest(result.Error),
                    HttpStatusCode.NotFound => Results.NotFound(result.Error),
                    _ => Results.Problem(result.Error)
                };
            });

            /// <summary>
            /// Delete a phone by id
            /// </summary>
            /// <param name="apiService"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            /// <response code="204">Phone deleted successfully</response>
            builder.MapDelete("/{id}", async (IMockApiService apiService, int id) =>
            {
                var result = await apiService.DeletePhone(id);
                if (result.IsSuccess)
                {
                    return Results.NoContent();
                }
                return result.HttpStatusCode switch
                {
                    HttpStatusCode.BadRequest => Results.BadRequest(result.Error),
                    HttpStatusCode.NotFound => Results.NotFound(result.Error),
                    _ => Results.Problem(result.Error)
                };
            });

            /// <summary>
            /// Get a phone by id
            /// </summary>
            /// <param name="apiService"></param>
            /// <param name="id"></param>
            builder.MapGet("/{id}", async (IMockApiService apiService, int id) =>
            {
                var result = await apiService.GetPhoneAsync(id);
                if (result.IsSuccess)
                {
                    return Results.Ok(result.Value);
                }
                return result.HttpStatusCode switch
                {
                    HttpStatusCode.BadRequest => Results.BadRequest(result.Error),
                    HttpStatusCode.NotFound => Results.NotFound(result.Error),
                    _ => Results.Problem(result.Error)
                };
            });

            return builder;
        }
    }
}
