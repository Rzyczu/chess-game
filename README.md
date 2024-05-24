# Chess Game

## About

This is a simple implementation of a chess game in C#. It provides a basic framework for playing chess with two players.

## Sctructure

- **Game**: The main class representing the game itself. It manages the game loop, players, turns, and interactions with the game board.
- **GameBoard**: Represents the game board and provides methods to add, remove, and access pieces on the board.
- **Player**: Represents a player in the game, including their color and score.
- **Turn**: Represents a single turn in the game, including the player making the move and the move itself.
- **Piece**: Represents a chess piece. It's an abstract class with subclasses for each type of piece (e.g., Pawn, Knight, etc.).
- **Coordinates**: Represents the coordinates of a position on the game board.
- **Move**: Represents a move made by a player, including the start and end positions, the piece played, any captured piece, and any promotion.

## Technologies Used

- C#: The primary language used for implementing the game logic.
- .NET Framework: Provides libraries and tools for building and running the application.



## Installation

1. Clone the repository to your local machine using Git:

```bash git clone https://github.com/rzyczu/chess-game.git`

## How to Play

ChessProject/bin/Debug/net7.0/chess.exe

## Features

- Basic chess rules are implemented, including movement rules for each type of piece and special moves like castling and pawn promotion.
- The game detects when a player's king is in check and prevents moves that would leave the king in check.
- The game tracks and displays the current turn, player, and game board state.
- When the game ends, the winner is declared, and the moves history is displayed.

## Dependencies

- This project does not have any external dependencies and can be run on any system with a C# compiler.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
