using AmbevTech._123Vendas.Application.Interfaces;
using AmbevTech._123Vendas.Application.Services;
using AmbevTech._123Vendas.Domain.Interfaces;
using AmbevTech._123Vendas.Domain.IoC;
using AmbevTech._123Vendas.Infra.DataContext;
using AmbevTech._123Vendas.Infra.EventBus;
using AmbevTech._123Vendas.Infra.Publisher;
using AmbevTech._123Vendas.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
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
        ConfigurarIdentity(services, configuration);
        ConfigurarServicos(services);
        ConfigurarRepositorios(services);
        ConfigurarAutoMapperConfiguration(services);
        ConfigurarGraphQL(services);
        ConfigurarSwagger(services);

    }

    private void ConfigurarIdentity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        });
    }

    private void ConfigurarSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AmbevTech.123Vendas",
                Version = "v1",
                Description = "Projeto para desafio técnico",
                Contact = new OpenApiContact
                {
                    Name = "Fabricio Silva da Rocha",
                    Email = "fsilvarocha@gmail.com",
                    Url = new Uri("https://github.com/fsilvarocha"),
                    Extensions = {
                    { "Telefone", new Microsoft.OpenApi.Any.OpenApiString("+55 (31) 984469354") }
                }
                }
            });
        });
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

