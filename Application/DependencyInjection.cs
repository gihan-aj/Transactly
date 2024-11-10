using Application.Abstractions.Behaviors;
using FluentValidation;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(
                config =>
                {
                    config.RegisterServicesFromAssembly(assembly);
                    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                    config.NotificationPublisher = new TaskWhenAllPublisher();
                });

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
