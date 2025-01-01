using System;
using System.Diagnostics;

namespace IdentityApi.Services
{

    public class AppLogger(IConfiguration _configuration) : ILogger, IAppLogger, IDisposable
    {

        private readonly List<IDisposable?> disposables = new List<IDisposable?>();


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            var disposable = default(IDisposable);

            disposables.Add(disposable);

            return disposable;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var configLevel = _configuration["Logging:LogLevel:Default"];
            var enabled = Enum.TryParse(configLevel, out LogLevel parsedLevel)
                ? parsedLevel >= logLevel
                : true;

            return enabled;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            WriteDate();
            WriteLevel(logLevel);
            WriteText(formatter(state, exception));
        }

        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                if (disposable == null)
                    continue;

                try
                {
                    disposable.Dispose();
                }
                catch { }
            }
        }

        public void Debug(string message)
        {
            if (!IsEnabled(LogLevel.Debug))
                return;

            var eventId = GetEventId();

            Log(LogLevel.Debug, eventId, message, null, 
                (message, exception) => message);
        }

        public void Information(string message)
        {
            if (!IsEnabled(LogLevel.Information))
                return;

            Log(LogLevel.Debug, default!, message, null, 
                (message, exception) => message);
        }

        public void Warning(string message)
        {
            if (!IsEnabled(LogLevel.Warning))
                return;

            Log(LogLevel.Debug, default!, message, null, 
                (message, exception) => message);
        }

        public void Error(Exception exception)
        {
            if (!IsEnabled(LogLevel.Error))
                return;

            Log(LogLevel.Debug, default!, exception.Message, exception, 
                (message, exception) => $"{exception}");
        }

        public void Error(string message)
        {
            if (!IsEnabled(LogLevel.Error))
                return;

            Log(LogLevel.Debug, default!, message, null,
                (message, exception) => message);
        }

        public void Error(Exception exception, string message)
        {
            if (!IsEnabled(LogLevel.Error))
                return;

            Log(LogLevel.Debug, default!, message, exception,
                (message, exception) => $"{message}: {exception}");
        }



        private void WriteDate() => Console.Write($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss.fff}] ");

        private void WriteLevel(LogLevel logLevel)
        {
            var defaultColor = ConsoleColor.Gray;

            Console.ForegroundColor = GetLevelColor(logLevel, defaultColor);
            Console.Write(logLevel.ToString());
            Console.ForegroundColor = defaultColor;
            Console.Write(" - ");
        }

        private ConsoleColor GetLevelColor(LogLevel logLevel, ConsoleColor defaultColor)
        {
            return logLevel switch
            {
                LogLevel.Debug => ConsoleColor.Blue,
                LogLevel.Information => ConsoleColor.Green,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Critical => ConsoleColor.Magenta,
                _ => defaultColor,
            };
        }

        private void WriteText(string text)
        {
            Console.WriteLine(text);
        }

        private EventId GetEventId()
        {
            var defaultEventId = new EventId(0, "Unknown");

            try
            {
                var stackTrace = new StackTrace();
                var frame = stackTrace.GetFrame(2); // 0 is GetCallingClassName, 1 is Log, 2 is the method that called Log, 3 is the caller of that method
                var method = frame.GetMethod();
                var declaringType = method.DeclaringType;
                var fullName = declaringType.FullName;
                var name = fullName.Split('+')[0];
                var id = GetId(name);
                var eventId = new EventId(id, name);

                return eventId;
            }
            catch
            {
                return defaultEventId;
            }
        }

        private int GetId(string name)
        {
            var i = 0;

            foreach (var c in name)
                i += (int)c;

            return i;
        }

    }



    public sealed class AppLoggerProvider(IConfiguration _configuration) : ILoggerProvider
    {

        private readonly AppLogger _logger = new AppLogger(_configuration);


        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
            _logger.Dispose();
        }

    }



    public static class AppLoggerExtensions
    {

        public static ILoggingBuilder ConfigureAppLogger(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.Services.AddSingleton<ILoggerProvider, AppLoggerProvider>();
            builder.Services.AddSingleton<IAppLogger, AppLogger>();

            return builder;
        }

    }

}
