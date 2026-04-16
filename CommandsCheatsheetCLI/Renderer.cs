namespace xyCmdCheatSheet;

internal static class Renderer
{
    public static void ShowAll()
    {
        foreach (var (key, entries) in AllEntries())
            ShowCategory(key, entries);
    }

    public static void ShowFiltered(string filter)
    {
        var all = AllEntries().ToList();
        var matches = all
            .Where(kv => kv.Key.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
        {
            Colored($"\n  No category found for: \"{filter}\"", ConsoleColor.Red);
            Colored($"  Available categories: {string.Join(", ", all.Select(kv => kv.Key))}\n", ConsoleColor.Yellow);
            return;
        }

        foreach (var (key, entries) in matches)
            ShowCategory(key, entries);
    }

    public static void ShowCategories()
    {
        Console.WriteLine();
        Colored("  Available categories:\n", ConsoleColor.Cyan);
        foreach (var key in AllEntries().Select(kv => kv.Key))
            Colored($"     - {key}", ConsoleColor.White);
        Console.WriteLine();
    }

    // -------------------------------------------------------------------------

    private static IEnumerable<KeyValuePair<string, List<CommandEntry>>> AllEntries()
    {
        foreach (var kv in Commands.StandardCommands)
            yield return kv;

        if (Commands.CustomCommands is not null)
            foreach (var kv in Commands.CustomCommands)
                yield return kv;
    }

    private static void ShowCategory(string category, List<CommandEntry> entries)
    {
        var bar = new string('─', 58);
        Console.WriteLine();
        Colored($"  {bar}", ConsoleColor.DarkGray);
        Colored($"  ## {category.ToUpperInvariant()}", ConsoleColor.Cyan);
        Colored($"  {bar}", ConsoleColor.DarkGray);

        foreach (var entry in entries)
        {
            Console.WriteLine();
            Colored($"  ### {entry.Title}", ConsoleColor.Yellow);
            foreach (var line in entry.Code.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                Colored($"      {line}", ConsoleColor.Green);
        }

        Console.WriteLine();
    }

    private static void Colored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}