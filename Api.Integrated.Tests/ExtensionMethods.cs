using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Api.Integrated.Tests
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Removes all registered <see cref="ServiceLifetime.Transient"/> registrations of <see cref="TService"/> and adds in <see cref="TImplementation"/>.
        /// </summary>
        /// <typeparam name="TService">The type of service interface which needs to be placed.</typeparam>
        /// <typeparam name="TImplementation">The test or mock implementation of <see cref="TService"/> to add into <see cref="services"/>.</typeparam>
        /// <param name="services"></param>
        public static void SwapTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
        {
            if (services.Any(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient))
            {
                var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Transient).ToList();
                foreach (var serviceDescriptor in serviceDescriptors)
                {
                    services.Remove(serviceDescriptor);
                }
            }

            services.AddTransient(typeof(TService), (sp) => implementationFactory(sp));
        }
    }
}
