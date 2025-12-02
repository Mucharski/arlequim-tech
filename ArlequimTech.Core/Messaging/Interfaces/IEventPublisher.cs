namespace ArlequimTech.Core.Messaging.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(string queueName, T message) where T : class;
}
