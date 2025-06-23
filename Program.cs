using System;

namespace ConnectFour
{
    // Entry point of the application
    class Program
    {
        static void Main(string[] args)
        {
            // Create game controller and start game loop
            GameController game = new GameController();
            game.StartGame();
        }
    }

    // Handles the overall game flow, switching turns, and managing game states
    public class GameController
    {
        private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public GameController()
        {
            board = new Board();
        }

        // Starts and loops the game until players choose to stop
        public void StartGame()
        {
            bool playAgain = true;

            while (playAgain)
            {
                // Ask user to choose between one-player or two-player mode
                Console.Write("Choose mode (1 = One-player, 2 = Two-player): ");
                string modeInput = Console.ReadLine();
                bool onePlayerMode = modeInput.Trim() == "1";

                // Player 1 is always human
                player1 = new HumanPlayer('X');

                // Player 2 is AI or Human depending on mode
                player2 = onePlayerMode ? new AIPlayer('O', board) : new HumanPlayer('O');

                board.Reset();
                currentPlayer = (new Random().Next(2) == 0) ? player1 : player2;
                Console.WriteLine($"Player {currentPlayer.Symbol} starts!");

                bool gameEnded = false;
                while (!gameEnded)
                {
                    // Show the current board
                    board.Display();
                    Console.WriteLine($"Turn: Player {currentPlayer.Symbol}");

                    int column = currentPlayer.GetMove();
                    if (!board.DropDisc(column, currentPlayer.Symbol))
                    {
                        Console.WriteLine("Invalid move! Try again.");
                        continue;
                    }

                    // Check if current player has won
                    if (board.CheckWin(currentPlayer.Symbol))
                    {
                        board.Display();
                        Console.WriteLine($"Player {currentPlayer.Symbol} WINS!");
                        gameEnded = true;
                    }
                    // Check if board is full (tie)
                    else if (board.IsFull())
                    {
                        board.Display();
                        Console.WriteLine("It's a TIE!");
                        gameEnded = true;
                    }
                    else
                    {
                        // Switch turns between players
                        currentPlayer = (currentPlayer == player1) ? player2 : player1;
                    }
                }

                Console.Write("Play again? (y/n): ");
                playAgain = Console.ReadLine().ToLower() == "y";
            }
        }
    }
// Represents the game board and its logic
    public class Board
    {
        private readonly char[,] grid;
        public const int Rows = 6, Columns = 7;
        private const char Empty = '.';

        public Board()
        {
            grid = new char[Rows, Columns];
            Reset();
        }

        // Reset the board to initial state
        public void Reset()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    grid[r, c] = Empty;
        }

        // Print the board to console
        public void Display()
        {
            Console.Clear();
            Console.WriteLine("1 2 3 4 5 6 7");
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                    Console.Write(grid[r, c] + " ");
                Console.WriteLine();
            }
        }

        // Drop disc into column, if valid
        public bool DropDisc(int col, char symbol)
        {
            if (col < 1 || col > Columns) return false;
            col--;
            for (int r = Rows - 1; r >= 0; r--)
                if (grid[r, col] == Empty)
                {
                    grid[r, col] = symbol;
                    return true;
                }
            return false;
        }

        // Check if the given player has a winning line
        public bool CheckWin(char symbol)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if ((c + 3 < Columns && grid[r, c] == symbol && grid[r, c + 1] == symbol && grid[r, c + 2] == symbol && grid[r, c + 3] == symbol)
                     || (r + 3 < Rows && grid[r, c] == symbol && grid[r + 1, c] == symbol && grid[r + 2, c] == symbol && grid[r + 3, c] == symbol)
                     || (r + 3 < Rows && c + 3 < Columns && grid[r, c] == symbol && grid[r + 1, c + 1] == symbol && grid[r + 2, c + 2] == symbol && grid[r + 3, c + 3] == symbol)
                     || (r - 3 >= 0 && c + 3 < Columns && grid[r, c] == symbol && grid[r - 1, c + 1] == symbol && grid[r - 2, c + 2] == symbol && grid[r - 3, c + 3] == symbol))
                        return true;
            return false;
        }
        // Check if selected column is not full
        public bool IsFull()
        {
            for (int c = 0; c < Columns; c++)
                if (grid[0, c] == Empty) return false;
            return true;
        }

        // Returns a copy of the board (used for AI simulations)
        public char[,] GetGridCopy()
        {
            return (char[,])grid.Clone();
        }

        // Check if a move is valid (column is not full)
        public bool IsValidMove(int col)
        {
            if (col < 1 || col > Columns) return false;
            return grid[0, col - 1] == Empty;
        }
    }

    // Base class for all players (human or AI)
    public abstract class Player
    {
        public char Symbol { get; }
        protected Player(char symbol) { Symbol = symbol; }
        public abstract int GetMove();
    }

    // Handles player input from console
    public class HumanPlayer : Player
    {
        public HumanPlayer(char symbol) : base(symbol) { }
        public override int GetMove()
        {
            int col;
            while (true)
            {
                Console.Write("Column (1–7): ");
                if (int.TryParse(Console.ReadLine(), out col) && col >= 1 && col <= 7)
                    return col;
                Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
            }
        }
    }
