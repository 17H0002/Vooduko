using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudukoGUI
{
    class Board
    {
        public int[,] PlayerBoard { get; set; }
        public int BoardSize { get; private set; }
        public int BlockLength { get; private set; }
        public int BlockHeight { get; private set; }

        private struct BlockDimensions
        {
            public int Vertical, Horizontal;
            public BlockDimensions(int boardSize)
            {
                switch (boardSize)
                {
                    case 4:
                        Vertical = 2;
                        Horizontal = 2;
                        break;
                    case 6:
                        Vertical = 2;
                        Horizontal = 3;
                        break;
                    case 9:
                        Vertical = 3;
                        Horizontal = 3;
                        break;
                    case 12:
                        Vertical = 3;
                        Horizontal = 4;
                        break;
                    case 16:
                        Vertical = 4;
                        Horizontal = 4;
                        break;
                    default:
                        Vertical = 3;
                        Horizontal = 3;
                        break;
                }
            }
        }
        public char BoardStyle;

        private void setBoard()
        {

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    PlayerBoard[row, col] = 0;

                }
            }
        }

        public override string ToString()
        {

            string output = "";
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    output += $" {PlayerBoard[row, col]} ";
                }
                output += "\n";
            }

            return output;
        }
        // 1 2 3
        // 4 5 6
        // 7 8 9
        // 1-2-3-/4-5-6-/7-8-9-
        public string ToElements()
        {
            string output = "";
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    output += $"{PlayerBoard[row, col]}-";
                }
                output += "/";
            }

            return output;
        }
        public Board(int size)
        {
            BoardSize = size;
            BlockDimensions blockDimensions = new BlockDimensions(size);
            BlockLength = blockDimensions.Horizontal;
            BlockHeight = blockDimensions.Vertical;
            PlayerBoard = new int[size, size];
            setBoard();
        }

        public Board(int size, char boardStyle)
        {
            BoardSize = size;
            BoardStyle = boardStyle;
            BlockDimensions blockDimensions = new BlockDimensions(size);
            BlockLength = blockDimensions.Horizontal;
            BlockHeight = blockDimensions.Vertical;
            PlayerBoard = new int[size, size];
            setBoard();
        }

        public Board(int [,] unplayedBoard)
        {
            BoardSize = unplayedBoard.GetLength(0);
            BlockDimensions blockDimensions = new BlockDimensions(BoardSize);
            BlockLength = blockDimensions.Horizontal;
            BlockHeight = blockDimensions.Vertical;
            PlayerBoard = unplayedBoard;
    
        }
    }
}
