namespace Movies.Client.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Responsible for adding the strongly typed custom configuration objects to the DI Container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddCustomAppConfiguration(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.Configure<OpenIdConnect>(configuration.GetSection("OpenIdConnect"));
            return services;
        }
    }
}
