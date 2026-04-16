namespace xyCmdCheatSheet;

internal class Program
{

    private static void Main(string[] args)
    {
        // cmd               → alles anzeigen
        // cmd git           → Kategorie filtern
        // cmd --list        → nur Kategorienamen

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

            case "--help" or "-h":
                Console.WriteLine("""
 
             Commands: 
         
             cmd                  Show all commands
             cmd <filter>       Filter category  (z.B. cmd git, cmd ef, cmd nuget)
             cmd --list           List all categories
             cmd --help         Show this help 
 
        """);
                break;

            default:
                Renderer.ShowFiltered(args[0]);
                break;
        }
        Console.ReadKey();
    }
}
