# WorldCupApp

Desktop application suite for exploring FIFA World Cup data, built with **.NET Framework 4.8** and split into:

- **WinForms client** for team/player browsing, favourites, drag-and-drop player management, and ranking exports.
- **WPF client** for match visualization, lineup rendering on a pitch, and detailed player/team insights.
- **DAL library** shared by both UIs for data access, storage, localization, and reporting.

## Solution layout

- `WorldCupApp.sln` – solution containing all projects.
- `DAL/` – shared data layer (models, data sources, services, file storage, PDF export).
- `WinForms/` – Windows Forms UI.
- `WPF/` – WPF UI.
- `Storage/` – persisted settings and local assets used at runtime.

## Main features

### Shared (DAL)

- Pulls teams and matches from either:
  - HTTP API: `https://worldcup-vua.nullbit.hr`
  - local JSON files (`Resources/Data/<Men|Women>/...`) via configurable data-source mode.
- Computes:
  - team players from match lineups
  - opponents list and head-to-head match lookup
  - goal rankings
  - yellow-card rankings
  - match attendance rankings
  - team summary stats (wins/draws/losses/goals)
- Persists application state in JSON files under `Storage/`:
  - `settings.json`
  - `favourites.json`
  - `playerImages.json`
  - `datasource.json`

### WinForms app

- First-run settings dialog (championship + language).
- Team selection with favourite-team persistence.
- Player rendering with drag-and-drop between “All players” and “Favourites”.
- Per-player image assignment with fallback image.
- Rankings window with tabs for goals, yellow cards, and attendance.
- PDF export for rankings.
- Confirm-close dialog.

### WPF app

- First-run settings dialog (championship + language + window mode).
- Team and opponent selection with last-opponent persistence per team.
- Match score display for selected matchup.
- Starting lineups rendered both as lists and as visual player cards on a pitch.
- Player details window and team summary windows.
- Fixed-size or fullscreen startup modes from settings.
- Confirm-exit dialog.

## Configuration

### 1) User settings and app state

Runtime state is stored in `Storage/` at the repository root.

### 2) Data source selection

Create or edit `Storage/datasource.json`:

```json
{
  "Mode": "Api",
  "JsonFolder": "Resources\\Data"
}
```

- `Mode` values:
  - `Api` (default)
  - `Json`
- `JsonFolder` is used when `Mode` is `Json` and is resolved relative to app base directory.

If the config is missing or invalid, the app falls back to API mode.

## Prerequisites

- Windows OS (WinForms/WPF desktop apps).
- Visual Studio 2022 (or MSBuild compatible with classic `.csproj` projects).
- .NET Framework 4.8 targeting pack.
- NuGet package restore enabled.

## Build and run

### Using Visual Studio

1. Open `WorldCupApp.sln`.
2. Restore NuGet packages (if not automatic).
3. Set startup project to either:
   - `WinForms`, or
   - `WPF`
4. Build and run (`F5`).


## Notes

- UI localization supports Croatian (`Hr`) and English (`En`).
- Some image fallback paths differ between WinForms/WPF implementations; keep `Storage/NoImage.jpg` available.
- PDF export is implemented via PDFsharp/MigraDoc referenced in DAL.
