namespace xyCmdCheatSheet;


/// <summary>
/// Contains the standard commands and optionally custom commands that can be added by the user.
/// </summary>
internal static class Commands
{
    internal record CommandEntry(string Title, string Code);

    public static string? Description { get; set; } = "Your ad here!!!";

    public static Dictionary<string, List<CommandEntry>>? CustomCommands { get; internal set; } = null;

    public static readonly Dictionary<string, List<CommandEntry>> StandardCommands =new(StringComparer.OrdinalIgnoreCase)
        {
            ["CLI"] =
            [
                new("(Later) Append info via echo >> to file, then commit",
                    """
                    echo "...." >> filename.txt
                    git add filename.txt
                    git commit -m "Ahuhu and Awawa!"
                    """),
            ],

            ["Directories"] =
            [
                new("Remove bin/obj (single project)",
                    "rm -rf bin obj"),

                new("Remove bin/obj (multiple projects in one solution)",
                    "Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force"),
            ],

            ["GIT"] =
            [
                new("Merge Branches into master",
                    """
                    git checkout master
                    git pull origin master
                    git merge target-branch
                    """),
            ],

            ["Nuget/ Dotnet tools"] =
            [
                new("Delete Cache",
                    "dotnet nuget locals all --clear"),

                new("Reload Dependencies",
                    """
                    dotnet clean
                    dotnet restore
                    dotnet build
                    """),

                new("Installation (local)",
                    """
                    dotnet new tool-manifest
                    dotnet tool install --local <toolname> --version <version>
                    """),

                new("Installation (global)",
                    "dotnet tool install --global <toolname> --version <version>"),

                new("Checking the Version of installed dotnet tools",
                    "dotnet tool list [--local | --global]"),

                new("Updating the Version",
                    "dotnet tool update <toolname> [--local | --global]"),
            ],

            ["EF Core"] =
            [
                new("Create DB Migration",
                    "dotnet ef migrations add <MigrationName> --project <...> --startup-project <...> --context <DbContextName>"),

                new("Create Database and migrate data into DB",
                    "dotnet ef database update --project <...> --startup-project <...> --context <DbContextName>"),
            ],

            ["XyDocGen"] =
            [
                new("Generate documentation (local)",
                    """dotnet xydocgen --root . --out docs/api --exclude ".git;bin;obj;node_modules;.vs;TestResults" """),

                new("Generate documentation (global)",
                    """xydocgen --root . --out docs/api --exclude ".git;bin;obj;node_modules;.vs;TestResults" """),
            ],
        };
}