# xyCmdCheatSheet

A lightweight .NET CLI tool (`xycmd`) to look up shell commands by category — with support for your own custom entries.

---

## Installation

```bash
# Global (recommended)
dotnet tool install --global xyCmdCheatSheet

# Local (per project)
dotnet new tool-manifest
dotnet tool install --local xyCmdCheatSheet
```

Requires **.NET 10** or later.

---

## Usage

```
xycmd                              Show all commands
xycmd <filter>                     Filter by category  (e.g. xycmd git, xycmd ef)
xycmd --list  | -l                 List all categories
xycmd --add <cat> <title> <code>   Add a custom command
xycmd --help  | -h                 Show help
```

**Examples**

```bash
xycmd git
xycmd ef
xycmd --list
xycmd --add GIT "Stash changes" "git stash"
xycmd --add Docker "Stop all containers" "docker stop $(docker ps -q)"
```

Multi-word arguments must be quoted.

---

## Built-in Categories

| Category          | Contains                                  |
|-------------------|-------------------------------------------|
| CLI               | General shell one-liners                  |
| Directories       | Remove `bin`/`obj` folders                |
| GIT               | Branch management                         |
| Nuget / Dotnet tools | Cache, restore, install, update        |
| EF Core           | Migrations, database update               |
| XyDocGen          | Documentation generation                  |

---

## Custom Commands

Custom entries are saved as JSON and persist across tool updates:

| OS      | Path                                                    |
|---------|---------------------------------------------------------|
| Windows | `%APPDATA%\xyCmdCheatSheet\custom-commands.json`        |
| Linux / macOS | `~/.config/xyCmdCheatSheet/custom-commands.json`  |

Custom entries in an existing category are merged with the built-in ones. New categories appear at the bottom.


## License

MIT