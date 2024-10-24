namespace AmbevTech._123Vendas.Domain.IoC;

public interface IResolver
{
    T GetService<T>();
}
