using AmbevTech._123Vendas.Domain.IoC;
using AmbevTech._123Vendas.Infra.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static AmbevTech._123Vendas.Domain.Helpers.Configurations;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace AmbevTech._123Vendas.Infra.IoC;

public class Resolver : IResolver
{
    private static IServiceProvider _serviceProvider;
    public Resolver(IServiceCollection services, IConfiguration configuration)
    {
        ConfigurarDI(services, configuration);
        ConfigurarServicos(services);
        ConfigurarRepositorios(services);
        ConfigurarAutoMapperConfiguration(services);
        ConfigurarGraphQL(services);
    }

    private void ConfigurarGraphQL(IServiceCollection services)
    {
    }

    private void ConfigurarAutoMapperConfiguration(IServiceCollection services)
    {
    }

    private void ConfigurarServicos(IServiceCollection services)
    {
    }

    private void ConfigurarRepositorios(IServiceCollection services)
    {
    }

    public Resolver()
    {
        ConfigurarDI(new ServiceCollection(), new ConfigurationManager());
    }

    public static void ConfigurarDI(IServiceCollection services, IConfiguration configuration)
    {
        ConfigurarDatabase(services);
    }

    private static void ConfigurarDatabase(IServiceCollection services)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(GetConfiguration<ConnectionString>("ConnectionString").Connection)
                                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            options.EnableSensitiveDataLogging();
        });

        services.AddTransient<Context>();
    }

    private static T GetConfiguration<T>(string secao) => new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build()
            .GetSection(secao)
            .Get<T>();

    public T GetService<T>()
    {
        throw new NotImplementedException();
    }
}
