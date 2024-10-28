using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace AmbevTech._123Vendas.Infra.DataContext;

public class ContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=AmbevTech;User ID=sa;Password=<SqlServerDocker!>;TrustServerCertificate=True;");

        return new Context(optionsBuilder.Options);
    }
}
