namespace MessageBus;

public interface IMessageBus
{

	IMessageBus ConsumeQueue<T>(string queueName, int fetchCount = 1, bool durable = false, bool exclusive = false, bool autoDelete = false);

	Task Run();

}
