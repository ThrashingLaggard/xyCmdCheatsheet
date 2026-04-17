using xyToolz.Helper.Logging;

namespace xyCmdCheatSheet;

internal class Program
{
    // xycmd                              → alles anzeigen
    // xycmd git                          → Kategorie filtern
    // xycmd --list                       → nur Kategorienamen
    // xycmd --add <cat> <title> <code>   → neuen Befehl hinzufügen
    // xycmd --help                       → Hilfe

    private static async Task Main(string[] args)
    {
        // Custom Commands beim Start aus dem persistenten JSON laden
        // und in Commands.CustomCommands bereitstellen (für Renderer.AllEntries)
        Commands.CustomCommands = await CommandStorage.LoadAsync();

        if (args.Length == 0)
        {
            Renderer.ShowAll();
            return;
        }

        switch (args[0].ToLowerInvariant())
        {
            case "--list" or "-l":
                Renderer.ShowCategories();
                break;

            case "--add" or "-a":
                await HandleAddAsync(args);
                break;

            case "--help" or "-h":
                PrintHelp();
                break;

            default:
                Renderer.ShowFiltered(args[0]);
                break;
        }
    }

    // -------------------------------------------------------------------------

    /// <summary>
    /// Handles the <c>--add</c> command: validates arguments, delegates persistence
    /// to <see cref="CommandStorage.AddEntryAsync"/>, and reports the result.
    /// </summary>
    /// <remarks>
    /// Expected usage: <c>xycmd --add &lt;category&gt; &lt;title&gt; &lt;code&gt;</c><br/>
    /// Multi-word arguments must be quoted in the shell, e.g.:<br/>
    /// <c>xycmd --add GIT "Stash changes" "git stash"</c>
    /// </remarks>
    private static async Task HandleAddAsync(string[] args)
    {
        if (args.Length < 4)
        {
            Console.Error.WriteLine(
                """

              Usage: xycmd --add <category> <title> <code>

              Examples:
                xycmd --add GIT       "Stash all changes"     "git stash"
                xycmd --add GIT       "Unstash"                   "git stash pop"
                xycmd --add Docker  "Stop all containers"   "docker stop $(docker ps -q)"

            """
            );
            Environment.Exit(1);
            return;
        }

        string category = args[1];
        string title = args[2];
        string code = args[3];

        if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(code))
        {
            xyLog.Log("\n  Error, category, title, and code must not be empty.\n");
            Environment.Exit(1);
            return;
        }

        bool saved = await CommandStorage.AddEntryAsync(category, title, code);

        if (saved)
            xyLog.Log($"\n  ✓  '{title}' was added to the category '{category}'.\n");
        else
            Console.Error.WriteLine($"\n  Error, '{title}' was not added to '{category}'\n");
    }

    private static void PrintHelp()
    {
        Console.WriteLine("""

         Commands:

         xycmd                                Show all commands
         xycmd <filter>                       Filter by category  (e.g. xycmd git, xycmd ef)
         xycmd --list                         List all categories
         xycmd --add <cat> <title> <code>     Add a custom command
         xycmd --help                         Show this help

        """);
    }
}