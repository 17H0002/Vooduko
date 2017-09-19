using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace noughtCrossGUI
{
    class Board
    {
        public char[,] PlayerBoard { get; set; }
        public int BoardSize { get; private set; }
        public char BoardStyle;

        public void setBoard()
        {

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    PlayerBoard[row, col] = BoardStyle;

                }
            }
        }

        public override string ToString()
        {

            string output = "\n";
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    output += $" {PlayerBoard[row, col]} ";
                }
                output += "\n\n";
            }

            return output;
        }

        public Board(int size, char boardStyle)
        {
            BoardSize = size;
            BoardStyle = boardStyle;
            PlayerBoard = new char[size, size];
            setBoard();
        }
    }
}
