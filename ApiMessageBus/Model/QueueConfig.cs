namespace MessageBus;

public class QueueConfig
{

	public string? QueueName { get; set; }

	public Type? QueueType { get; set; }

	public int FetchCount { get; set; }

	public bool Durable { get; set; }

	public bool Exclusive { get; set; }

	public bool AutoDelete { get; set; }

}
