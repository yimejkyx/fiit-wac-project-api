using eu.fiit.PatientsPortal.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    static class DataRepositoryDI
    {
        /// supportive extension function for dependency
        /// injection of the service
        public static IServiceCollection AddDataRepository(
            this IServiceCollection services)
            => services.AddSingleton<IDataRepository, DataRepository>();
    }
}