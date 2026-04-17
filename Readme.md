# 🐍 Snake

A classic Snake game built with **C# / WPF** using the **MVVM** architectural pattern.

## Features

- Smooth snake movement with collision detection (walls and self)
- Randomized apple spawning (1–6 apples at a time)
- Sound effects on food consumption and death
- Game pause (Esc)
- Dynamic window title based on score milestones
- Leaderboard with nickname, score, grid size, speed, and date
- Delete individual records from the leaderboard
- Theme with animated UI elements (XamlFlair)
- Logging system with per-session log files

## Controls

| Key | Action |
|---|---|
| W / ↑ | Move Up |
| S / ↓ | Move Down |
| A / ← | Move Left |
| D / → | Move Right |
| Esc | Pause / Resume |

## Settings

Before starting a game you can configure:

- **Nickname** — shown in the leaderboard
- **Grid size** — 7×7, 10×10, 20×20, or 30×30
- **Speed** — from slow to fast via a slider

## Leaderboard

Records are saved automatically after each game (only if score > 0). The leaderboard is sorted by score descending and shows:

- Nickname
- Score
- Grid size
- Speed
- Date and time

Individual records can be deleted using the **remove** button.

## Screenshots

<img width="400" alt="image" src="https://github.com/user-attachments/assets/54e80cd9-9291-40ed-af33-a43068a140d5" />
<img width="400" alt="image" src="https://github.com/user-attachments/assets/eaf4bfb6-958a-40ff-af67-d8d97a56ccee" />
<img width="400" alt="image" src="https://github.com/user-attachments/assets/66bd8436-0808-424f-a9d3-1ce11f27b9bb" />

## License

MIT
