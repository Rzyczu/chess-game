# Chess Game

## About

This is a simple implementation of a console chess game in C#. It provides a basic framework for playing chess with two players.

![image](https://github.com/user-attachments/assets/3bc7b0b2-9114-45a1-b5f8-7b498fc97821)


## Technologies Used

- C#: The primary language used for implementing the game logic.
- .NET 7.0 Framework: Provides libraries and tools for building and running the application.

## Installation

1. Clone the repository to your local machine using Git:

``bash git clone https://github.com/rzyczu/chess-game.git

## How to Play

ChessProject/bin/Debug/net7.0/chess.exe

## Features

- Basic chess rules are implemented:
  - Piece movement
  - Capturing
  - Castling
  - Promotion
  - Check
  - Checkmate
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
- **GameBoard**: Represents the game board and provides methods to add, remove, and access pieces on the board.
- **Move**: Represents a move made by a player, including the start and end positions, the piece played, any captured piece, and any promotion.
- **Player**: Represents a player in the game, including their color and score.
- **Turn**: Represents a single turn in the game, including the player making the move and the move itself.
- **Piece**: Represents a chess piece. It's an abstract class with subclasses for each type of piece (King, Queen, Rook, Bishop, Knight, Pawn).
- **ColorType**: Enum representing the color of pieces (Black or White).
- **PieceType**: Enum representing the type of chess pieces (King, Queen, Rook, Bishop, Knight, Pawn).
- **ConsoleHelper**: Provides helper methods for writing messages to the console with different colors.
- **ErrorMessages**: Contains static error messages used throughout the game.
- **InfoMessages**: Contains static informational messages used throughout the game.
- **WarningMessages**: Contains static warning messages used throughout the game.
- **PieceSymbols**: Contains a dictionary mapping piece types to their corresponding Unicode symbols.

### External Libraries

The project does not rely on any external libraries or dependencies.
