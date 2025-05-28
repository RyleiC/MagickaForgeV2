namespace ContentCompiler.Misc
{
    internal class Logger
    {
        private const ConsoleColor WarningColor = ConsoleColor.DarkYellow;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor PromptColor = ConsoleColor.Yellow;
        private const ConsoleColor ResultColor = ConsoleColor.Cyan;
        private const ConsoleColor TitleColor = ConsoleColor.Blue;
        private const ConsoleColor InfoColor = ConsoleColor.Gray;
        private const ConsoleColor SuccessColor = ConsoleColor.Green;

        private static void WriteLineColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(text);

            Console.ResetColor();
        }

        public static void WriteWarning(string text)
        {
            WriteLineColored(text, WarningColor);
        }

        public static void WriteError(string text)
        {
            WriteLineColored(text, ErrorColor);
        }

        public static void WritePrompt(string text)
        {
            WriteLineColored(text, PromptColor);
        }

        public static void WriteResult(string text)
        {
            WriteLineColored(text, ResultColor);
        }

        public static void WriteTitle(string text)
        {
            WriteLineColored(text, TitleColor);
        }

        public static void WriteInfo(string text)
        {
            WriteLineColored(text, InfoColor);
        }

        public static void WriteSuccess(string text)
        {
            WriteLineColored(text, SuccessColor);
        }
    }
}
