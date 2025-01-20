namespace LoggerModels;

public interface IAppLogger
{

    void Debug(string message);

    void Information(string message);

    void Warning(string message);

    void Error(Exception exception);

    void Error(string message);

    void Error(Exception exception, string message);

}
