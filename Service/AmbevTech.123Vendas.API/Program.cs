using AmbevTech._123Vendas.API.Middleware;
using AmbevTech._123Vendas.Domain.IoC;
using AmbevTech._123Vendas.IoC.IoC;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"Log/Log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


try
{
    Log.Warning("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "AmbevTeck.123Vendas",
            Version = "v1",
            Description = "Projeto para desafio t�cnico",
            Contact = new OpenApiContact
            {
                Name = "Fabricio Silva da Rocha",
                Email = "fsilvarocha@gmail.com",
                Url = new Uri("https://github.com/fsilvarocha"), 
                Extensions = {
                {"Telefone", new Microsoft.OpenApi.Any.OpenApiString("+55 (31) 984469354")} 
            }
            }
        });
    });

    Dependencias.Resolver = new Resolver(builder.Services, builder.Configuration);

    builder.Services.AddExceptionHandler<ErrorHandlingMiddleware>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.Warning("Shutting down");
    Log.CloseAndFlush();
}


