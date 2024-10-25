using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Application.Services;
using AmbevTech._123Vendas.Domain.Interfaces;
using AmbevTech._123Vendas.Domain.IoC;
using AmbevTech._123Vendas.Infra.DataContext;
using AmbevTech._123Vendas.Infra.EventBus;
using AmbevTech._123Vendas.Infra.Publisher;
using AmbevTech._123Vendas.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tingle.EventBus;
using static AmbevTech._123Vendas.Domain.Helpers.Configurations;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace AmbevTech._123Vendas.IoC.IoC;

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
        services.AddScoped<IVendaService, VendaService>();
        services.AddScoped<IEventBus, EventBusRabbitMQ>();
        services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
    }

    private void ConfigurarRepositorios(IServiceCollection services)
    {
        services.AddScoped<IVendaRepository, VendaRepository>();
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

