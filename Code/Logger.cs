namespace PurTools
{
    public abstract class Logger
    {
        private static Logger _current = new ConsoleLogger();

        /// <summary>
        /// The currently used Logger, Or a ConsoleLogger if no current logger is set
        /// </summary>
        public static Logger Current
        {
            get => _current ?? new ConsoleLogger();
            set => _current = value;
        }

        /// <summary>
        /// Logs a informational message
        /// </summary>
        /// <param name="message">Message to log</param>
        public abstract void Info(string message);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">Message to log</param>
        public abstract void Warn(string message);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">Message to log</param>
        public abstract void Error(string message);
    }
}
