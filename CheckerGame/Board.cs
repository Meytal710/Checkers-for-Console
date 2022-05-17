using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_EX02
{
    public class Board
    {
        private readonly int r_Size;
        private char[,] m_GameBoard;
        private static StringBuilder s_BoardAsString;

        public Board(int i_Size)
        {
            this.r_Size = i_Size;
            this.m_GameBoard = new char[r_Size, r_Size];
            int numOfLinesPerPlayer = (r_Size - 2) / 2;

            for (int i = 0; i < numOfLinesPerPlayer; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        this.m_GameBoard[i, j] = 'O';
                    }
                    else
                    {
                        this.m_GameBoard[i, j] = ' ';
                    }
                }
            }
            for (int i = numOfLinesPerPlayer; i < numOfLinesPerPlayer + 2; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    this.m_GameBoard[i, j] = ' ';
                }

            }
            for (int i = numOfLinesPerPlayer + 2; i < i_Size; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        this.m_GameBoard[i, j] = 'X';
                    }
                    else
                    {
                        this.m_GameBoard[i, j] = ' ';
                    }
                }

            }
        }

        public int Size
        {
            get { return r_Size; }
        }

        public void PrintBoard()
        {
            Screen.Clear();
            int size = this.r_Size;

            s_BoardAsString = new StringBuilder("");
            for (int i = 0; i < size; i++)
            {
                s_BoardAsString.Append(string.Format("   {0}", (char)(i + 65)));
            }

            seperateRow(size);
            for (int i = 0; i < size; i++)
            {
                s_BoardAsString.Append(string.Format(@"{0}{1}", Environment.NewLine, (char)(i + 97)));
                for (int j = 0; j < size; j++)
                {
                    s_BoardAsString.Append(string.Format(@" | {0}", this.m_GameBoard[i, j]));
                }

                s_BoardAsString.Append(" | ");
                seperateRow(size);
            }

            s_BoardAsString.Append(Environment.NewLine);
            Console.WriteLine(s_BoardAsString.ToString());

        }

        private static void seperateRow(int i_Size)
        {
            s_BoardAsString.Append(String.Format(@"{0}  ", Environment.NewLine));
            for (int i = 0; i < i_Size; i++)
            {
                s_BoardAsString.Append("====");
            }

            s_BoardAsString.Append("=");
        }

        public char[,] GetGameBoard()
        {
            return this.m_GameBoard;
        }

        public List<Tuple<int, int, int, int>> PossibleMoves(char i_Sign)
        {
            List<Tuple<int, int, int, int>> possibleMoves = new List<Tuple<int, int, int, int>> { };

            for (int row = 0; row < r_Size; row++)
            {
                for (int col = 0; col < r_Size; col++)
                {
                    if (i_Sign == 'X') 
                    { 
                    if (m_GameBoard[row, col] == 'X' || m_GameBoard[row, col] == 'Z')
                    {
                        //moving up right without eating
                        if (isNotOverEdges(row - 1, col + 1))
                        {
                            if (m_GameBoard[row - 1, col + 1] == ' ')
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 1, col + 1));
                            }
                        }
                        //moving up left without eating
                        if (isNotOverEdges(row - 1, col - 1))
                        {
                            if (m_GameBoard[row - 1, col - 1] == ' ')
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 1, col - 1));
                            }
                        }

                        //moving up right eating
                        if (isNotOverEdges(row - 2, col + 2))
                        {
                            if ((m_GameBoard[row - 1, col + 1] == 'O' || m_GameBoard[row - 1, col + 1] == 'Q') && (m_GameBoard[row - 2, col + 2] == ' '))
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 2, col + 2));
                            }
                        }

                        //moving up left eating
                        if (isNotOverEdges(row - 2, col - 2))
                        {
                            if ((m_GameBoard[row - 1, col - 1] == 'O' || m_GameBoard[row - 1, col - 1] == 'Q') && (m_GameBoard[row - 2, col - 2] == ' '))
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 2, col - 2));
                            }
                        }

                        if (m_GameBoard[row, col] == 'Z')
                        {
                            //moving down right without eating
                            if (isNotOverEdges(row + 1, col + 1))
                            {
                                if (m_GameBoard[row + 1, col + 1] == ' ')
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 1, col + 1));
                                }
                            }

                            //moving down left without eating
                            if (isNotOverEdges(row + 1, col - 1))
                            {
                                if (m_GameBoard[row + 1, col - 1] == ' ')
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 1, col - 1));
                                }
                            }

                            //moving down right eating
                            if (isNotOverEdges(row + 2, col + 2))
                            {
                                if ((m_GameBoard[row + 1, col + 1] == 'O' || m_GameBoard[row + 1, col + 1] == 'Q') && (m_GameBoard[row + 2, col + 2] == ' '))
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 2, col + 2));
                                }
                            }

                            //moving up left eating
                            if (isNotOverEdges(row + 2, col - 2))
                            {
                                if ((m_GameBoard[row + 1, col - 1] == 'O' || m_GameBoard[row + 1, col - 1] == 'Q') && (m_GameBoard[row + 2, col - 2] == ' '))
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 2, col - 2));
                                }
                            }
                        }
                    }
                    }
                    if (i_Sign == 'O')
                    { 
                    if (m_GameBoard[row, col] == 'O' || m_GameBoard[row, col] == 'Q')
                    {
                        //moving down right without eating
                        if (isNotOverEdges(row + 1, col + 1))
                        {
                            if (m_GameBoard[row + 1, col + 1] == ' ')
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 1, col + 1));
                            }
                        }
                        //moving down left without eating
                        if (isNotOverEdges(row + 1, col - 1))
                        {
                            if (m_GameBoard[row + 1, col - 1] == ' ')
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 1, col - 1));
                            }
                        }
                        //moving down right eating
                        if (isNotOverEdges(row + 2, col + 2))
                        {
                            if ((m_GameBoard[row + 1, col + 1] == 'X' || m_GameBoard[row + 1, col + 1] == 'Z') && (m_GameBoard[row + 2, col + 2] == ' '))
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 2, col + 2));
                            }
                        }
                        //moving down left eating
                        if (isNotOverEdges(row + 2, col - 2))
                        {
                            if ((m_GameBoard[row + 1, col - 1] == 'X' || m_GameBoard[row + 1, col - 1] == 'Z') && (m_GameBoard[row + 2, col - 2] == ' '))
                            {
                                possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row + 2, col - 2));
                            }
                        }
                        if (m_GameBoard[row, col] == 'Q')
                        {
                            //moving up right without eating
                            if (isNotOverEdges(row - 1, col + 1))
                            {
                                if (m_GameBoard[row - 1, col + 1] == ' ')
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 1, col + 1));
                                }
                            }
                            //moving up left without eating
                            if (isNotOverEdges(row - 1, col - 1))
                            {
                                if (m_GameBoard[row - 1, col - 1] == ' ')
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 1, col - 1));
                                }
                            }
                            //moving up right eating
                            if (isNotOverEdges(row - 2, col + 2))
                            {
                                if ((m_GameBoard[row - 1, col + 1] == 'O' || m_GameBoard[row - 1, col + 1] == 'Q') && (m_GameBoard[row - 2, col + 2] == ' '))
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 2, col + 2));
                                }
                            }
                            //moving up left eating
                            if (isNotOverEdges(row - 2, col - 2))
                            {
                                if ((m_GameBoard[row - 1, col - 1] == 'O' || m_GameBoard[row - 1, col - 1] == 'Q') && (m_GameBoard[row - 2, col - 2] == ' '))
                                {
                                    possibleMoves.Add(new Tuple<int, int, int, int>(row, col, row - 2, col - 2));
                                }
                            }
                        }
                    }
                }

            }

        }
            return possibleMoves;
        }

        private bool isNotOverEdges(int i_Row, int i_Col)
        {
            if (i_Col >= r_Size || i_Row >= r_Size || i_Col < 0 || i_Row < 0)
            {
                return false;
            }

            return true;
        }
    }
}

