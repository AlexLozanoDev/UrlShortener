using Api.Common.Errors;
using Api.Common.Errors.ExceptionMappers;

namespace Api.Extensions
{
    public static class ExceptionHandlingExtensions
    {

        public static IServiceCollection AddApiExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddProblemDetails();

            services.AddSingleton<IExceptionMapper, DomainExceptionMapper>();
            services.AddSingleton<IExceptionMapper, AppExceptionMapper>();
            services.AddSingleton<IExceptionMapper, PersistenceExceptionMapper>();
            services.AddSingleton<IExceptionMapper, FrameworkExceptionMapper>();
            services.AddSingleton<IExceptionMapper, CacheExceptionMapper>();
            services.AddSingleton<IExceptionMapper, ExternalServiceExceptionMapper>();

            return services;
        }

        public static WebApplication UseApiErrorHandling(this WebApplication app)
        {
            app.UseExceptionHandler();
            return app;
        }
    }
}
