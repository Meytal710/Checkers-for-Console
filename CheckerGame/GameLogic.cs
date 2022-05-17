using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_EX02
{
    class GameLogic
    {
        public static bool s_GameOver = false;
        public static List<Tuple<int, int, int, int>> s_ValidMoves = new List<Tuple<int, int, int, int>> { };

        public static void CountScore(Player i_WinningPlayer, Player i_LosingPlayer, Board i_Board)
        {
            int winningCounter = 0;
            int losingCounter = 0;

            foreach (char c in i_Board.GetGameBoard())
            {
                if (c == i_WinningPlayer.Sign)
                {
                    winningCounter++;
                }
                if (c == i_LosingPlayer.Sign)
                {
                    losingCounter++;
                }
                if (c == i_WinningPlayer.King)
                {
                    winningCounter += 4;
                }
                if (c == i_LosingPlayer.King)
                {
                    losingCounter += 4;
                }
            }

            i_WinningPlayer.Score += Math.Abs(winningCounter - losingCounter);
        }

        public static void DoMove(Tuple<int, int, int, int, string> i_Move, Board i_Board)
        {
            bool hasMoved = false;
            int startRow = i_Move.Item1;
            int startCol = i_Move.Item2;
            int endRow = i_Move.Item3;
            int endCol = i_Move.Item4;
            char currentSign = i_Board.GetGameBoard()[startRow, startCol];
            
            Console.WriteLine(currentSign);
            if (!s_ValidMoves.Any())
            {
                if (currentSign == 'X')
                {
                    if (!hasMoved && movingUpWithoutEating('X', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingUpRightEating('X', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingUpLeftEating('X', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }
                }

                if (currentSign == 'O')
                {
                    if (!hasMoved && movingDownWithoutEating('O', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingDownRightEating('O', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingDownLeftEating('O', startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }
                }
                if (currentSign == 'Z' || currentSign == 'Q')
                {
                    if (!hasMoved && movingUpWithoutEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingUpRightEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingUpLeftEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingDownWithoutEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingDownRightEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }

                    if (!hasMoved && movingDownLeftEating(currentSign, startRow, startCol, endRow, endCol, i_Board.GetGameBoard()))
                    {
                        hasMoved = true;
                    }
                }

                i_Board.PrintBoard();
            }
        }

        public static bool isNotOverEdges(int i_Col, int i_Row, char[,] i_Board)
        {
            bool isValid = true;

            if ((i_Col >= i_Board.GetLength(1)) || (i_Row >= i_Board.GetLength(0)) || i_Col < 0 || i_Row < 0)
            {
                isValid = false;
                isValid = false;
            }

            return isValid;
        }

        // return a list of tuples of valid moves after eating.
        // if the list is empty there is no valid move and the turn is now the next player
        private static List<Tuple<int, int, int, int>> nextValidMoveAfterEating(int i_Col, int i_Row, char i_Sign, char[,] i_Board)
        {
            char signToEat1 = ' ';
            char signToEat2 = ' ';

            if (i_Sign == 'Q' || i_Sign == 'O')
            {
                signToEat1 = 'X';
                signToEat2 = 'Z';
            }

            if (i_Sign == 'Z' || i_Sign == 'X')
            {
                signToEat1 = 'O';
                signToEat2 = 'Q';
            }

            if (i_Sign == 'X')
            {
                doubleEatingUpLeft(signToEat1, signToEat2, i_Row, i_Col, i_Board);
                doubleEatingUpRight(signToEat1, signToEat2, i_Row, i_Col, i_Board);
            }

            if (i_Sign == 'O')
            {
                doubleEatingDownLeft(signToEat1, signToEat2, i_Row, i_Col, i_Board);
                doubleEatingDownRight(signToEat1, signToEat2, i_Row, i_Col, i_Board);
            }

            if (i_Sign == 'Q' || i_Sign == 'Z')
            {
                doubleEatingDownLeft(signToEat1, signToEat2, i_Row, i_Col, i_Board);
                doubleEatingDownRight(signToEat1, signToEat2, i_Row, i_Col, i_Board);
                doubleEatingUpLeft(signToEat1, signToEat2, i_Row, i_Col, i_Board);
                doubleEatingUpRight(signToEat1, signToEat2, i_Row, i_Col, i_Board);
            }

            return s_ValidMoves;
        }

        private static void isKing(char i_Sign, int i_Row, int i_Col, char[,] i_Board)
        {
            if (i_Sign == 'X')
            {
                if (i_Row == 0)
                {
                    i_Board[i_Row, i_Col] = 'Z';
                }
            }

            if (i_Sign == 'O')
            {
                if (i_Row == i_Board.GetLength(0) - 1)
                {
                    i_Board[i_Row, i_Col] = 'Q';
                }
            }
        }

        private static bool movingUpWithoutEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            bool hasMoved = false;

            //moving up one step without eating
            if (((i_StartCol == i_EndCol + 1) || (i_StartCol == i_EndCol - 1)) && (i_StartRow == i_EndRow + 1))
            {
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                hasMoved = true;
            }

            return hasMoved;
        }

        private static bool movingUpRightEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            bool hasMoved = false;
            char signToEat1 = ' ';
            char signToEat2 = ' ';

            if (i_Sign == 'Q')
            {
                signToEat1 = 'X';
                signToEat2 = 'Z';
            }

            if (i_Sign == 'Z' || i_Sign == 'X')
            {
                signToEat1 = 'O';
                signToEat2 = 'Q';
            }

            if ((i_StartCol == i_EndCol - 2) && (i_StartRow == i_EndRow + 2) && (i_Board[i_StartRow - 1, i_StartCol + 1] == signToEat1 || i_Board[i_StartRow - 1, i_StartCol + 1] == signToEat2))
            {
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_StartRow - 1, i_StartCol + 1] = ' ';
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                nextValidMoveAfterEating(i_EndCol, i_EndRow, i_Sign, i_Board);
                hasMoved = true;
            }
            return hasMoved;
        }

        private static bool movingUpLeftEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            char signToEat1 = ' ';
            char signToEat2 = ' ';
            bool hasMoved = false;

            if (i_Sign == 'Q')
            {
                signToEat1 = 'X';
                signToEat2 = 'Z';
            }

            if (i_Sign == 'Z' || i_Sign == 'X')
            {
                signToEat1 = 'O';
                signToEat2 = 'Q';
            }

            if ((i_StartCol == i_EndCol + 2) && (i_StartRow == i_EndRow + 2) && (i_Board[i_StartRow - 1, i_StartCol - 1] == signToEat1 || i_Board[i_StartRow - 1, i_StartCol - 1] == signToEat2))
            {
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_StartRow - 1, i_StartCol - 1] = ' ';
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                nextValidMoveAfterEating(i_EndCol, i_EndRow, i_Sign, i_Board);
                hasMoved = true;
            }

            return hasMoved;
        }

        private static bool movingDownWithoutEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            bool hasMoved = false;

            if (((i_StartCol == i_EndCol + 1) || (i_StartCol == i_EndCol - 1)) && (i_StartRow == i_EndRow - 1))
            {
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                hasMoved = true;
            }

            return hasMoved;
        }

        private static bool movingDownRightEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            char signToEat1 = ' ';
            char signToEat2 = ' ';
            bool hasMoved = false;

            if (i_Sign == 'O' || i_Sign == 'Q')
            {
                signToEat1 = 'X';
                signToEat2 = 'Z';
            }

            if (i_Sign == 'Z')
            {
                signToEat1 = 'O';
                signToEat2 = 'Q';
            }

            if ((i_StartCol == i_EndCol - 2) && (i_StartRow == i_EndRow - 2) && (i_Board[i_StartRow + 1, i_StartCol + 1] == signToEat1 || i_Board[i_StartRow + 1, i_StartCol + 1] == signToEat2))
            {
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_StartRow + 1, i_StartCol + 1] = ' ';
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                nextValidMoveAfterEating(i_EndCol, i_EndRow, i_Sign, i_Board);
                hasMoved = true;
            }

            return hasMoved;
        }

        private static bool movingDownLeftEating(char i_Sign, int i_StartRow, int i_StartCol, int i_EndRow, int i_EndCol, char[,] i_Board)
        {
            char signToEat1 = ' ';
            char signToEat2 = ' ';
            bool hasMoved = false;

            if (i_Sign == 'O' || i_Sign == 'Q')
            {
                signToEat1 = 'X';
                signToEat2 = 'Z';
            }

            if (i_Sign == 'Z')
            {
                signToEat1 = 'O';
                signToEat2 = 'Q';
            }

            if ((i_StartCol == i_EndCol + 2) && (i_StartRow == i_EndRow - 2) && (i_Board[i_StartRow + 1, i_StartCol - 1] == signToEat1 || i_Board[i_StartRow + 1, i_StartCol - 1] == signToEat2))
            {
                i_Board[i_EndRow, i_EndCol] = i_Sign;
                i_Board[i_StartRow, i_StartCol] = ' ';
                i_Board[i_StartRow + 1, i_StartCol - 1] = ' ';
                isKing(i_Sign, i_EndRow, i_EndCol, i_Board);
                nextValidMoveAfterEating(i_EndCol, i_EndRow, i_Sign, i_Board);
                hasMoved = true;
            }

            return hasMoved;
        }

        private static void doubleEatingUpLeft(char i_SignToEat1, char i_SignToEat2, int i_Row, int i_Col, char[,] i_Board)
        {
            if (isNotOverEdges(i_Col - 2, i_Row - 2, i_Board))
            {
                if ((i_Board[i_Row - 1, i_Col - 1] == i_SignToEat1 || i_Board[i_Row - 1, i_Col - 1] == i_SignToEat2) && (i_Board[i_Row - 2, i_Col - 2] == ' '))
                {
                    s_ValidMoves.Add(new Tuple<int, int, int, int>(i_Row, i_Col, i_Row - 2, i_Col - 2));
                }
            }
        }

        private static void doubleEatingUpRight(char i_SignToEat1, char i_SignToEat2, int i_Row, int i_Col, char[,] i_Board)
        {
            if (isNotOverEdges(i_Col + 2, i_Row - 2, i_Board))
            {
                if ((i_Board[i_Row - 1, i_Col + 1] == i_SignToEat1 || i_Board[i_Row - 1, i_Col + 1] == i_SignToEat2) && (i_Board[i_Row - 2, i_Col + 2] == ' '))
                {
                    s_ValidMoves.Add(new Tuple<int, int, int, int>(i_Row, i_Col, i_Row - 2, i_Col + 2));
                }
            }
        }

        private static void doubleEatingDownLeft(char i_SignToEat1, char i_SignToEat2, int i_Row, int i_Col, char[,] i_Board)
        {
            if (isNotOverEdges(i_Col - 2, i_Row + 2, i_Board))
            {
                if ((i_Board[i_Row + 1, i_Col - 1] == i_SignToEat1 || i_Board[i_Row + 1, i_Col - 1] == i_SignToEat2) && (i_Board[i_Row + 2, i_Col - 2] == ' '))
                {
                    s_ValidMoves.Add(new Tuple<int, int, int, int>(i_Row, i_Col, i_Row + 2, i_Col - 2));
                }
            }
        }

        private static void doubleEatingDownRight(char i_SignToEat1, char i_SignToEat2, int i_Row, int i_Col, char[,] i_Board)
        {
            if (isNotOverEdges(i_Col + 2, i_Row + 2, i_Board))
            {
                if ((i_Board[i_Row + 1, i_Col + 1] == i_SignToEat1 || i_Board[i_Row + 1, i_Col + 1] == i_SignToEat2) && (i_Board[i_Row + 2, i_Col + 2] == ' '))
                {
                    s_ValidMoves.Add(new Tuple<int, int, int, int>(i_Row, i_Col, i_Row + 2, i_Col + 2));
                }
            }
        }

        public static bool DoDoubleMove(Tuple<int, int, int, int, string> i_Move, Player i_CurrentPlayer, Board i_Board)
        {
            bool notMoved = true;
            int startRow = i_Move.Item1;
            int startCol = i_Move.Item2;
            int endRow = i_Move.Item3;
            int endCol = i_Move.Item4;

            if (s_ValidMoves.Any())
            {
                for (int i = 0; i < s_ValidMoves.Count; i++)
                {
                    if (s_ValidMoves[i].Item1 == startRow && s_ValidMoves[i].Item2 == startCol && s_ValidMoves[i].Item3 == endRow && s_ValidMoves[i].Item4 == endCol)
                    {
                        i_Board.GetGameBoard()[startRow, startCol] = ' ';
                        int deleteCol = (startCol + endCol) / 2;
                        int deleteRow = (startRow + endRow) / 2;
                        i_Board.GetGameBoard()[deleteRow, deleteCol] = ' ';
                        if (i_CurrentPlayer.Sign == 'X')
                        {
                            i_Board.GetGameBoard()[endRow, endCol] = 'X';
                            isKing('X', endRow, endCol, i_Board.GetGameBoard());
                        }

                        if (i_CurrentPlayer.Sign == 'O')
                        {
                            i_Board.GetGameBoard()[endRow, endCol] = 'O';
                            isKing('O', endRow, endCol, i_Board.GetGameBoard());
                        }

                        if (i_CurrentPlayer.King == 'Q')
                        {
                            i_Board.GetGameBoard()[endRow, endCol] = 'Q';
                        }

                        if (i_CurrentPlayer.King == 'Z')
                        {
                            i_Board.GetGameBoard()[endRow, endCol] = 'Z';
                        }

                        notMoved = false;
                        s_ValidMoves.Clear();
                    }
                }
            }
            i_Board.PrintBoard();
            return notMoved;
        }
    }
}

   



