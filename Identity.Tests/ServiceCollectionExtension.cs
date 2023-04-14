using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Identity.Tests
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            if (services.IsReadOnly)
            {
                throw new ReadOnlyException($"{nameof(services)} is read only");
            }

            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);

            return services;
        }
    }
}
