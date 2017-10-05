using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudukoGUI
{
    class Game
    {
        public enum playerType { player1, player2, computerAI }
        public string gameName { get; set; }
        public string[] playerNames { get; set; }
        public playerType player { get; set; }
        public Board Board { get; set; }
        public int gameMode { get; set; } //gameMode = 0 ~ Single player sudoku. gameMode = 1 ~ Two player sudoku. gameMode = 2 ~ Single player suduko against the computer.
        public bool gameOn { get; set; } //gameOn = false ~ game has ended. gameOn = true ~ game is in play.
        public int numMoves { get; set; }
        public string message { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int move { get; set; }

        public void check1()
        {
            if (Board.PlayerBoard[row, col] == 0 && gameOn == false)
            {
                gameOn = true;
                Board.PlayerBoard[row, col] = move;


            }
        }
        public void check2()
        {
            if (gameOn == true)
            {
                int dupcnt = 0;

                for (int tt = 0; tt < Board.BoardSize; tt++)
                {
                    if (Board.PlayerBoard[row, tt] == move)
                    {
                        dupcnt = dupcnt + 1;

                    }
                }
                if (dupcnt == 2)
                {

                    Board.PlayerBoard[row, col] = 0;
                    //error = 4;
                    gameOn = false;
                    throw new MainWindow.RowDupeException();
                }
            }
        }
        public void check3()
        {
            if (gameOn == true)
            {
                int dupcnt = 0;

                for (int tt = 0; tt < Board.BoardSize; tt++)
                {
                    if (Board.PlayerBoard[tt, col] == move)
                    {
                        dupcnt = dupcnt + 1;
                    }
                }
                if (dupcnt == 2)
                {
                    //error = 5;
                    Board.PlayerBoard[row, col] = 0;
                    gameOn = false;
                    throw new MainWindow.ColDupeException();
                }
            }
        }
        public void check4()
        {
            if (gameOn == true)
            {
                int RBound = (row / Board.BlockLength) * Board.BlockLength;
                int CBound = (col / Board.BlockHeight) * Board.BlockHeight;
                int[] bline;
                int ic = 0;
                int bi = Board.BlockLength * Board.BlockHeight;
                bline = new int[bi];

                for (int rc = 0; rc < Board.BlockLength; rc++)
                {
                    for (int cc = 0; cc < Board.BlockHeight; cc++)
                    {
                        bline[ic] = Board.PlayerBoard[RBound, CBound];
                        ic = ic + 1;
                        CBound = CBound + 1;

                    }
                    RBound = RBound + 1;
                    CBound = (col / Board.BlockHeight) * Board.BlockLength;
                }
                int dupcnt = 0;

                for (int tt = 0; tt < bi; tt++)
                {
                    if (bline[tt] == move)
                    {
                        dupcnt = dupcnt + 1;
                    }
                }
                if (dupcnt == 2)
                {
                    //error = 6;
                    Board.PlayerBoard[row, col] = 0;
                    gameOn = false;
                    throw new MainWindow.BlockDupeException();
                }
            }
        }
        //Check procedures.
        public void check(int moveValue)
        {
            move = moveValue;
            check1();
            check2();
            check3();
            check4();
        }
        //Initialiser for all sudoku games.
        public Game()
        {

        }
        //Initialiser for loading previous games.
        public Game(string name, string[] players, playerType p, Board b, int mode, bool on, int moves, string m, int r, int mc, int mov)
        {
            gameName = name;
            playerNames = players;
            player = p;
            Board = b;
            gameMode = mode;
            gameOn = on;
            numMoves = moves;
            message = m;
            row = r;
            col = mc;
            move = mov;
        }
    }
}
