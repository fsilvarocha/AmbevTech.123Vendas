namespace AmbevTech._123Vendas.Domain.Exception;

public class BusinessException : IOException
{
    public BusinessException(string message) : base(message)
    {

    }
}
