using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Tingle.EventBus;
using IModel = RabbitMQ.Client.IModel;

namespace AmbevTech._123Vendas.Infra.Publisher;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqEventPublisher()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task<ScheduledResult?> PublishAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEvent>(EventContext<TEvent> @event, DateTimeOffset? scheduled = null, CancellationToken cancellationToken = default) where TEvent : class
    {
        if (@event != null)
        {
            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "", routingKey: eventName, basicProperties: null, body: body);

            return Task.FromResult(new ScheduledResult?());
        }
        else return null;
    }

    public Task<IList<ScheduledResult>?> PublishAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEvent>(IList<EventContext<TEvent>> events, DateTimeOffset? scheduled = null, CancellationToken cancellationToken = default) where TEvent : class
    {
        return null;
    }


    public Task CancelAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEvent>(string id, CancellationToken cancellationToken = default) where TEvent : class
    {
        return null;
    }

    public Task CancelAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEvent>(IList<string> ids, CancellationToken cancellationToken = default) where TEvent : class
    {
        return null;
    }

    public EventContext<TEvent> CreateEventContext<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TEvent>(TEvent @event, string? correlationId = null) where TEvent : class
    {
        return null;
    }
}
