namespace RestWebAPI.Extensions
{
    public static class MockAPIMethodExtensions
    {
        public static IEndpointRouteBuilder RouteMockAPIEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGroup("/mockdata");
            builder.MapGet("/", () =>
            {

            });

            return builder;
        }
    }
}
