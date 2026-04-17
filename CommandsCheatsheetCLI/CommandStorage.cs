using xyToolz.Filesystem;
using xyToolz.Serialization;
using static xyCmdCheatSheet.Commands;

namespace xyCmdCheatSheet;

/// <summary>
/// Handles persistent storage of user-defined custom commands.
/// </summary>
/// <remarks>
/// Storage location (platform-specific application data folder):
/// <list type="bullet">
///   <item>Windows: <c>%APPDATA%\xyCmdCheatSheet\custom-commands.json</c></item>
///   <item>Linux/macOS: <c>~/.config/xyCmdCheatSheet/custom-commands.json</c></item>
/// </list>
/// File access is delegated to <see cref="xyFiles"/> and <see cref="xyJson"/>.
/// </remarks>
internal static class CommandStorage
{
    /// <summary>
    /// Full path to the JSON storage file.
    /// Placed in the OS-specific ApplicationData folder so that the file
    /// survives tool reinstalls and is writable without elevated permissions.
    /// </summary>
    private static readonly string StoragePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"xyCmdCheatSheet","custom-commands.json");

    // -------------------------------------------------------------------------

    /// <summary>
    /// Loads all custom commands from the JSON storage file.
    /// </summary>
    /// <remarks>
    /// Uses <see cref="xyJson.DeserializeFromFile{T}"/> for deserialization.
    /// The returned dictionary always uses <see cref="StringComparer.OrdinalIgnoreCase"/>
    /// to match the behaviour of <c>Commands.StandardCommands</c>.
    /// If the file does not exist yet, an empty dictionary is returned without error.
    /// </remarks>
    /// <returns>
    /// A case-insensitive dictionary of categories → command lists,
    /// or an empty dictionary when the storage file is missing or unreadable.
    /// </returns>
    internal static async Task<Dictionary<string, List<CommandEntry>>> LoadAsync()
    {
        // xyJson.DeserializeFromFile uses xyFiles.GetStreamFromFileAsync internally.
        // It returns default(T) – i.e. null – when the file does not exist yet,
        // so we fall back to an empty dictionary in that case.
        var raw = await xyJson.DeserializeFromFile<Dictionary<string, List<CommandEntry>>>(StoragePath);

        // System.Text.Json creates dictionaries with Ordinal comparer by default.
        // Wrap the result so category lookups stay case-insensitive.
        return new Dictionary<string, List<CommandEntry>>(raw ?? [],StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Persists the given command dictionary to the JSON storage file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="xyPath.EnsureParentDirectoryExists"/> is called before
    /// <see cref="xyJson.SaveDataToJsonAsync{T}"/> because the latter uses
    /// <c>xyFiles.EnsurePathExistsAsync</c> internally, which calls <c>File.Create</c>
    /// without creating the parent directory – resulting in a
    /// <see cref="DirectoryNotFoundException"/> on first use.
    /// </para>
    /// </remarks>
    /// <param name="commands">The full set of custom commands to persist.</param>
    /// <returns>True if the file was written successfully; otherwise, false.</returns>
    internal static async Task<bool> SaveAsync(Dictionary<string, List<CommandEntry>> commands)
    {
        // Ensure %APPDATA%\xyCmdCheatSheet\ exists before xyJson tries to create the file.
        xyPath.EnsureParentDirectoryExists(StoragePath);

        return await xyJson.SaveDataToJsonAsync(commands, StoragePath);
    }

    /// <summary>
    /// Adds a new <see cref="CommandEntry"/> to the specified category and saves the result.
    /// Creates the category automatically if it does not exist yet.
    /// </summary>
    /// <param name="category">Target category (case-insensitive).</param>
    /// <param name="title">Short heading shown in the output.</param>
    /// <param name="code">The CLI command(s) to display. Multi-line supported.</param>
    /// <returns>True if the entry was saved successfully; otherwise, false.</returns>
    internal static async Task<bool> AddEntryAsync(string category, string title, string code)
    {
        var commands = await LoadAsync();

        if (!commands.TryGetValue(category, out var list))
        {
            list = [];
            commands[category] = list;
        }

        list.Add(new CommandEntry(title, code));

        return await SaveAsync(commands);
    }
}