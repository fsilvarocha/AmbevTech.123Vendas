using AmbevTech._123Vendas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AmbevTech._123Vendas.Infra.DataContext;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;


        var mappingTypes = Assembly.GetExecutingAssembly().GetTypes()
           .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
           .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        foreach (var mappingType in mappingTypes)
        {
            dynamic instance = Activator.CreateInstance(mappingType);
            modelBuilder.ApplyConfiguration(instance);
        }

        base.OnModelCreating(modelBuilder);

    }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; }

}