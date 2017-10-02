using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dooko
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        class board
        {
            public board()
            {

            }
        }
        class game
        {
            public string gameName { get; set; }
            public string[] playerNames { get; set; } //Array of 2 strings; player 1 and player2. When playing against the computer, player2 is called "AI".
            public board Board { get; set; }
            public int gameMode { get; set; } //gameMode = 0 ~ Single player sudoku. gameMode = 1 ~ Two player sudoku.
            public int gameOn { get; set; } //gameOn = 0 ~ game has ended. gameOn = 1 ~ game is in play.
            public int numMoves { get; set; } //Allows the recording of the number of moves: for if numMoves is even (including 0), player1 is to move ; if numMoves is odd, player 2 is to move.
            //Initialiser for all sudoku games.
            public game(string name, string[] player, board b, int mode)
            {
                gameName = name;
                playerNames = player;
                Board = b;
                gameMode = mode;
                gameOn = 1;
                numMoves = 0;
            }
            //Initialiser for loading previous games.
            public game(string name, string[] player, board b, int mode, int on, int moves)
            {
                gameName = name;
                playerNames = player;
                Board = b;
                gameMode = mode;
                gameOn = on;
                numMoves = moves;
            }
        }
        public void move()
        {
            //Contains relevant code concerning the procedure for the computer to make a move.
        }
        public bool check1()
        {
            //Contains relevant code concerning the first check procedure.
            return true;
        }
        public bool check2()
        {
            //Contains relevant code concerning the second check procedure.
            return true;
        }
        public bool check3()
        {
            //Contains relevant code concerning the third check procedure.
            return true;
        }
        public bool check4()
        {
            //Contains relevant code concerning the fourth check procedure.
            return true;
        }
        //Check procedures.
        public bool check()
        {
            if(check1() == false && check2() == false && check3() == false && check4() == false)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
