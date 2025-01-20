namespace MessageBus;

public interface IQueue<T>
{

	Task Process(T data);

}

public interface IQueue
{

	Task Process();

}
