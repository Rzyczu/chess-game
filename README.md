# Chess Game

## About

This is a simple implementation of a chess game in C#. It provides a basic framework for playing chess with two players.

## Technologies Used

- C#: The primary language used for implementing the game logic.
- .NET Framework: Provides libraries and tools for building and running the application.

## Installation

1. Clone the repository to your local machine using Git:

``bash git clone https://github.com/rzyczu/chess-game.git

## How to Play

ChessProject/bin/Debug/net7.0/chess.exe

## Features

- Basic chess rules are implemented, including movement rules for each type of piece and special moves like castling and pawn promotion.
- The game detects when a player's king is in check and prevents moves that would leave the king in check.
- The game tracks and displays the current turn, player, and game board state.
- When the game ends, the winner is declared, and the moves history is displayed.

## Project Structure

The project follows a typical C# class library structure, organized into namespaces and classes:

### Namespaces

- **chess**: Contains the main classes related to the chess game.
- **chess.Components**: Contains classes representing various components of the chess game, such as pieces, the game board, players, and turns.
- **chess.Helpers**: Contains helper classes and enums used throughout the project.
- **chess.Helpers.Enums**: Contains enums used to represent colors and types of chess pieces.

### Classes and Enums

- **Game**: Represents the main game logic, including initialization, game flow, and move execution.
- **Coordinates**: Represents the coordinates on the game board.
- **GameBoard**: Represents the game board and provides methods for managing pieces and checking board boundaries.
- **Move**: Represents a move made by a player.
- **Player**: Represents a player in the game.
- **Turn**: Represents a turn taken by a player.
- **ColorType**: Enum representing the color of pieces (Black or White).
- **PieceType**: Enum representing the type of chess pieces (King, Queen, Rook, Bishop, Knight, Pawn).
- **ConsoleHelper**: Provides helper methods for writing messages to the console with different colors.
- **ErrorMessages**: Contains static error messages used throughout the game.
- **InfoMessages**: Contains static informational messages used throughout the game.
- **WarningMessages**: Contains static warning messages used throughout the game.
- **PieceSymbols**: Contains a dictionary mapping piece types to their corresponding Unicode symbols.

### External Libraries

The project does not rely on any external libraries or dependencies.
