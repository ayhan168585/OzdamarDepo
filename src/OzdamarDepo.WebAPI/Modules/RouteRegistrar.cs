namespace OzdamarDepo.WebAPI.Modules
{
    public static class RouteRegistrar
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder app)
        {
            app.RegisterMediaItemRoutes();
            app.RegisterAuthRoutes();

        }
    }
}
