namespace AmbevTech._123Vendas.Domain.IoC;

public class Dependencias
{
    private static IResolver _resolver;
    private static readonly object _lock = new();

    public static IResolver Resolver
    {
        get
        {
            lock (_lock)
            {
                if (_resolver == null)
                    throw new System.Exception("Nenhuma instância de \"IResolvedor\" foi configurada no domínio.");

                return _resolver;
            }
        }

        set
        {
            lock (_lock)
            {
                _resolver = value;
            }
        }
    }
}


