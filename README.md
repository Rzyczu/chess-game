# Chess – Console Chess Game (C# / .NET 7)

A clean, object‑oriented **console chess engine + CLI** in **C#/.NET 7**. Implements core chess rules with per‑piece validation, **check detection**, **checkmate end condition**, **castling**, **pawn promotion**, path‑blocking logic, Unicode board rendering, and a simple algebraic‑style **move history**.  

---

## Tech Stack

- **Language/Runtime:** C# (NET 7.0)
- **App type:** Console application
- **Core modules:**
  - `Components/` – board, coordinates, move, player, turn
  - `Components/Pieces/` – King, Queen, Rook, Bishop, Knight, Pawn (per‑piece validation)
  - `Helpers/` – console I/O, messages, enums, formatting helpers (algebraic ↔ coordinates)
- **Entry point:** `chessProject/Program.cs`

---

## Features (from code)

- **Two‑player local play** (alternating turns, session in terminal).
- **Board rendering** with Unicode piece symbols and ANSI colors.
- **Per‑piece move validation** (`Piece.IsValidMove(...)`) and **path obstruction** checks for sliding pieces.
- **Check** detection (`IsKingInCheck`) and **game over** when the checked side **cannot escape** (`CanEscapeCheck`).
- **Castling** validation (king/rook not moved, empty path).
- **Pawn promotion** with in‑console choice (Q/R/B/N) on reaching last rank.
- **Move history** printed at the end (`FormatMove` → `e2-e4`, captures `x`, promotion `=Q`, piece letters `KQRBN`).

> **Known limitations (by design in this version):**
> - **En passant** is not implemented.
> - **Stalemate/draw** conditions are not explicitly handled (game ends on checkmate condition only).
> - No persistence, AI, or network play.

---

## How to Run

**Prerequisites:** .NET 7 SDK

```bash
# from repository root
dotnet run --project chessProject
```

You should see the welcome banner, current player info, and a rendered board.

---

## Input Format (CLI)

- Moves are entered as **algebraic coordinates**: `"<from> <to>"` (space‑separated), e.g.:
  - `e2 e4`, `b1 c3`
- **Castling:** move the **king** two squares:
  - White: `e1 g1` (king‑side), `e1 c1` (queen‑side)
  - Black: `e8 g8`, `e8 c8`
- **Promotion:** when a pawn reaches last rank, the app asks to pick a piece: `1‑4` (Q, R, B, N).

**Invalid input** or illegal moves trigger descriptive error messages and a retry.

---

## Project Structure (high‑level)

```
chessProject/
  Program.cs
  Game.cs
  Components/
    GameBoard.cs
    Coordinates.cs
    Move.cs
    Player.cs
    Turn.cs
    Pieces/
      Piece.cs
      Pawn.cs
      Rook.cs
      Knight.cs
      Bishop.cs
      Queen.cs
      King.cs
  Helpers/
    ConsoleHelper.cs
    Messages.cs
    PieceSymbols.cs
    FormatHelper.cs
    Enums/
      ChessColor.cs
      PieceType.cs
  UnitTests/ (sample, commented)
```

---

## Screen

![image](https://github.com/user-attachments/assets/a6d386d7-5648-4c04-a4b5-63a3f5cd1a92)

---

## Notes

- Board coordinates are 0‑indexed internally; input/output uses **`a1..h8`** style mapping.
- The engine prints **turn history** after the game finishes.
- Customize console colors or symbols via `GameBoard.PrintBoard()` and `Helpers/PieceSymbols.cs`.
- The code targets **.NET 7**; adjust `TargetFramework` if you need a different runtime.

---

## License

MIT (see repository).
