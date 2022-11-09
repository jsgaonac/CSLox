using System.Runtime.CompilerServices;

namespace CSLox;

public class CSLox
{
    private static bool HadError;
    
    public static void Main(string[] args)
    {
        switch (args.Length)
        {
            case > 1:
                Console.WriteLine("Usage: CSLox [script]");
                Environment.Exit(64);
                break;
            case 1:
                RunFile(args[0]);
                break;
            default:
                RunPrompt();
                break;
        }
    }

    private static void RunFile(string path)
    {
        var lines = File.ReadAllText(path);

        Run(lines);
        
        if (HadError)
            Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();

            if (line is null)
                break;

            Run(line);

            HadError = false;
        }
    }

    private static void Run(string line)
    {
    }

    public static void Error(int line, string message)
    {
        Report(line, string.Empty, message);
    }

    private static void Report(int line, string where, string message)
    {
       Console.WriteLine($"[Line {line}] Error {where}: {message}");

       HadError = true;
    }
}
