namespace MessageBus;

public interface IPublisher
{

    Task Publish<T>(Message<T> message, string queueName, string exchange = "");

		Task Publish(Message message, string queueName, string exchange = "");

	}
