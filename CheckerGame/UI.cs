using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_EX02
{
    class UI
    {
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Board m_Board;
        private string m_ComputerOrPlayer;

        public void run()
        {
            m_FirstPlayer = new Player(getPlayerName(1), 'X', 'Z', 0);
            m_Board = new Board(getBoardSize());

            getComputerOrPlayer();
            if (m_ComputerOrPlayer == "p")
            {
                m_SecondPlayer = new Player(getPlayerName(2), 'O', 'Q', 0);
            } 
            else
            {
                m_SecondPlayer = new Player("computer", 'O', 'Q', 0);
            }

            gamePlay(m_FirstPlayer, m_SecondPlayer, m_Board);

        }

        private static string getPlayerName(int i_PlayerNumber)
        {
            if (i_PlayerNumber == 1)
            {
                Console.WriteLine("Hi! Please enter your name (no spaces, max 10 chars)");
            }
            else
            {
                Console.WriteLine("Please enter player 2 name (no spaces, max 10 chars)");
            }
            string name = Console.ReadLine();

            while (!checkIfValidName(name))
            {
                Console.WriteLine("The name you entered is illegal. Please enter a valid name (no spaces, max 10 chars)");
                name = Console.ReadLine();
            }

            return name;
        }

        private static bool checkIfValidName(string i_name)
        {
            bool isValid = true;

            if (i_name.Length > 10)
            {
                isValid = false;
            }

            foreach (char c in i_name)
            {
                if (c == ' ')
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private static int getBoardSize()
        {
            Console.WriteLine("Please choose board size (6,8 or 10)");
            string boardSizeString = Console.ReadLine();

            while (boardSizeString != "6" && boardSizeString != "8" && boardSizeString != "10")
            {
                Console.WriteLine("illegal board size. Please enter a valid size (6,8 or 10)");
                boardSizeString = Console.ReadLine();
            }

            int.TryParse(boardSizeString, out int boardIntOutput);

            return boardIntOutput;
        }

        private void getComputerOrPlayer()
        {
            Console.WriteLine("Would you like to play against the computer or another player. Press 'c' for computer or 'p' for player");
            string computerOrPlayer = Console.ReadLine();

            while (computerOrPlayer != "c" && computerOrPlayer != "p")
            {
                Console.WriteLine("illegal entry. please choose 'c' for computer or 'p' for player");
                computerOrPlayer = Console.ReadLine();
            }

            this.m_ComputerOrPlayer = computerOrPlayer;
        }

        // already checked if the player has a valid move to play
        private static bool checkIfValidMove(string i_Move, Player i_Player, Board i_Board)
        {
            bool checkValid = false;

            if (i_Move.Length == 5) {

                int startRow = i_Move[1] - 97;
                int endRow = i_Move[4] - 97;
                int startCol = i_Move[0] - 65;
                int endCol = i_Move[3] - 65;
                Tuple<int, int, int, int> currentMove = new Tuple<int, int, int, int> (startRow, startCol, endRow, endCol);
                List<Tuple<int, int, int, int>> playerMoves = i_Board.PossibleMoves(i_Player.Sign);

                for (int i = 0; i < playerMoves.Count; i++)
                {
                    if (i_Move[2] == '>' && (playerMoves[i].Item1 == currentMove.Item1) && (playerMoves[i].Item2 == currentMove.Item2) && (playerMoves[i].Item3 == currentMove.Item3) && (playerMoves[i].Item4 == currentMove.Item4))
                    {
                        checkValid = true;
                    }
                }
            }

            if (checkValid == false)
            {
                Console.WriteLine("invalid move! " + i_Player.Name + "'s turn (" + i_Player.Sign + "):");
                
            }

            return checkValid;
        }

        private void gameOvered(Player i_WinningPlayer, Player i_LosingPlayer, Board i_Board)
        {
            GameLogic.CountScore(i_WinningPlayer, i_LosingPlayer, i_Board);
            Console.WriteLine(i_WinningPlayer.Name + " wins!!!!");
            Console.WriteLine(i_WinningPlayer.Name + "'s score: " + i_WinningPlayer.Score);
            Console.WriteLine(i_LosingPlayer.Name + "'s score: " + i_LosingPlayer.Score);
            Console.WriteLine("Would you like to play another game? (Y/N)");
            string answer = Console.ReadLine();

            while (answer != "Y" && answer != "N")
            {
                Console.WriteLine("Invalid entry! Would you like to play another game? (Y/N)");
                answer = Console.ReadLine();
            }

            if (answer == "Y")
            {
                gamePlay(m_FirstPlayer, m_SecondPlayer, m_Board);
            }

            if (answer == "N")
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        private Tuple<int, int, int, int, string> getMoveFromPlayer(Player i_CurrentPlayer, Player i_OtherPlayer, Board i_Board)
        {
            List<Tuple<int, int, int, int>> playerMoves = i_Board.PossibleMoves(i_CurrentPlayer.Sign);
            Tuple<int, int, int, int, string> moveTuple = new Tuple<int, int, int, int, string>(-1, -1, -1, -1, "-1");

            if (!playerMoves.Any())
            {
                gameOvered(i_OtherPlayer, i_CurrentPlayer, i_Board);
            }
            else
            {
                Console.WriteLine(i_CurrentPlayer.Name + "'s turn (" + i_CurrentPlayer.Sign + "):");
                string currentMove = Console.ReadLine();

                if (currentMove == "Q")
                {
                    gameOvered(i_OtherPlayer, i_CurrentPlayer, i_Board);
                }

                while (!checkIfValidMove(currentMove, i_CurrentPlayer, i_Board))
                {
                    currentMove = Console.ReadLine();
                    if (currentMove == "Q")
                    {
                        gameOvered(i_OtherPlayer, i_CurrentPlayer, i_Board);
                    }
                }

                int startRow = currentMove[1] - 97;
                int endRow = currentMove[4] - 97;
                int startCol = currentMove[0] - 65;
                int endCol = currentMove[3] - 65;
                moveTuple = new Tuple<int, int, int, int, string>(startRow, startCol, endRow, endCol, currentMove);

            }

            return moveTuple;
        }

        private void gamePlay(Player i_FirstPlayer, Player i_SecondPlayer, Board i_Board) 
        {
            Board newBoard = new Board(i_Board.Size);
            newBoard.PrintBoard();
            bool gameOver = false;
            
            while (!gameOver)
            {
                // first player
                List<Tuple<int, int, int, int>> firstPlayerMoves = newBoard.PossibleMoves('X');
                if (!firstPlayerMoves.Any())
                {
                    gameOvered(i_SecondPlayer, i_FirstPlayer, newBoard);
                    gameOver = true;
                    break;
                }

                Tuple<int, int, int, int, string> firstPlayerMoveTuple = getMoveFromPlayer(i_FirstPlayer, i_SecondPlayer, newBoard);
                
                GameLogic.DoMove(firstPlayerMoveTuple, newBoard);
                Console.WriteLine(i_FirstPlayer.Name + "'s move was " + firstPlayerMoveTuple.Item5);
                while (GameLogic.s_ValidMoves.Any())
                {
                    Tuple<int, int, int, int, string> firstPlayerDoubleMoveTuple = getMoveFromPlayer(i_FirstPlayer, i_SecondPlayer, newBoard);
                    
                    if (GameLogic.DoDoubleMove(firstPlayerDoubleMoveTuple, i_FirstPlayer, newBoard))
                    {
                        Console.WriteLine("Invalid move! You must continue with the same pond.");
                    }
                }

                // second player
                List<Tuple<int, int, int, int>> secondPlayerMoves = newBoard.PossibleMoves('O');
                
                if (!secondPlayerMoves.Any())
                {
                    gameOvered(i_SecondPlayer, i_FirstPlayer, newBoard);
                    gameOver = true;
                    break;
                }

                if (m_ComputerOrPlayer == "c")
                {
                    Console.WriteLine(i_SecondPlayer.Name + "'s turn (O):");
                    Random rnd = new Random();

                    if (!secondPlayerMoves.Any())
                    {
                        Console.WriteLine(i_FirstPlayer.Name + " wins!!!!");
                        gameOvered(i_FirstPlayer, i_SecondPlayer, newBoard);
                        gameOver = true;
                    }

                    int index = rnd.Next(secondPlayerMoves.Count);
                    char startColComputer = Convert.ToChar(secondPlayerMoves[index].Item2 + 65);
                    char startRowComputer = Convert.ToChar(secondPlayerMoves[index].Item1 + 97);
                    char endColComputer = Convert.ToChar(secondPlayerMoves[index].Item4 + 65);
                    char endRowComputer = Convert.ToChar(secondPlayerMoves[index].Item3 + 97);
                    char bigger = '>';
                    string computerMove = string.Format(@"{0}{1}{2}{3}{4}", startColComputer, startRowComputer, bigger, endColComputer, endRowComputer);
                    Tuple<int, int, int, int, string> computerMoveTuple = new Tuple<int, int, int, int, string>(secondPlayerMoves[index].Item1, secondPlayerMoves[index].Item2, secondPlayerMoves[index].Item3, secondPlayerMoves[index].Item4, computerMove);
                    
                    GameLogic.DoMove(computerMoveTuple, newBoard);
                    Console.WriteLine(i_SecondPlayer.Name + "'s move was " + computerMoveTuple.Item5);
                    while (GameLogic.s_ValidMoves.Any())
                    {
                        index = rnd.Next(GameLogic.s_ValidMoves.Count);
                        startRowComputer = Convert.ToChar(secondPlayerMoves[index].Item1 + 65);
                        startColComputer = Convert.ToChar(secondPlayerMoves[index].Item2 + 97);
                        endRowComputer = Convert.ToChar(secondPlayerMoves[index].Item3 + 65);
                        endColComputer = Convert.ToChar(secondPlayerMoves[index].Item4 + 97);
                        bigger = '>';
                        computerMove = string.Format(@"{0}{1}{2}{3}{4}", startRowComputer, startColComputer, bigger, endRowComputer, endColComputer);
                        computerMoveTuple = new Tuple<int, int, int, int, string>(secondPlayerMoves[index].Item1, secondPlayerMoves[index].Item2, secondPlayerMoves[index].Item3, secondPlayerMoves[index].Item4, computerMove);

                        GameLogic.DoDoubleMove(computerMoveTuple, i_SecondPlayer, newBoard);
                    }
                }
                else
                {
                    Tuple<int, int, int, int, string> secondPlayerMoveTuple = getMoveFromPlayer(i_SecondPlayer, i_FirstPlayer, newBoard);
                    
                    GameLogic.DoMove(secondPlayerMoveTuple, newBoard);
                    Console.WriteLine(i_SecondPlayer.Name + "'s move was " + secondPlayerMoveTuple.Item5);
                    while (GameLogic.s_ValidMoves.Any())
                    {
                        Tuple<int, int, int, int, string> secondPlayerDoubleMoveTuple = getMoveFromPlayer(i_SecondPlayer, i_FirstPlayer, newBoard);
                        if (GameLogic.DoDoubleMove(secondPlayerDoubleMoveTuple, i_FirstPlayer, newBoard))
                        {
                            Console.WriteLine("Invalid move! You must continue with the same pond.");
                        }
                    }
                }
            }
        }
    }  
}
    
