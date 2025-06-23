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
