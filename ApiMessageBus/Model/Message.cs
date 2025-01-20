namespace MessageBus;

public class Message<T>
{

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public T? Data { get; set; }


    public Message()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public Message(T data) : this()
    {
        Data = data;
    }

}


public class Message
{

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

}