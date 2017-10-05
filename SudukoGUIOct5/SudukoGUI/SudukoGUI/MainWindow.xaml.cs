using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;

namespace SudukoGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Board> boards = new List<Board>() { };
        List<Game> games = new List<Game>() { };
        Game currentGame;
       
        private StreamWriter w;
        private StreamReader r;
        //private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private Button button;
        private Button[] buttons;
        private int counter;
        int minSelect = 1;


        public MainWindow()
        {
            InitializeComponent();
            currentGame = new Game();
            currentGame.gameName = "Test";
            currentGame.playerNames = new string[2] { "String1", "String2"};
            currentGame.player = Game.playerType.player1;
            currentGame.gameMode = 0;
            currentGame.gameOn = false;
            currentGame.numMoves = 0;
            currentGame.message = "";
            currentGame.row = 0;
            currentGame.col = 0;
            currentGame.move = 0;
            currentGame.Board = new Board(9, ' ');
            runGame();
            
            InitialiseBoardButtons();
        }

        public class RowDupeException : Exception
        {   
            public RowDupeException() : base("Illegal move! - Duplicate Value in row!") {

            }
        }
        public class ColDupeException : Exception
        {
            public ColDupeException() : base("Illegal move! - Duplicate Value in column!")
            {

            }
        }
        public class BlockDupeException : Exception
        {
            public BlockDupeException() : base("Illegal move! - Duplicate Value in block!")
            {

            }
        }
        public class OccupiedTileException : Exception
        {
            public OccupiedTileException() : base("Hey! You have already played a move here!")
            {

            }
        }

        //Suduko
        private struct BoardSizeChange
        {
            public int Vertical, Horizontal;
            public bool Condition;
            public BoardSizeChange(Board b, int row, int col)
            {
                switch (b.BoardSize)
                {
                    case 4:
                        Vertical = 1;
                        Horizontal = 1;
                        Condition = !(row == 2 || col == 2);
                        break;
                    case 6:
                        Vertical = 1;
                        Horizontal = 2;
                        Condition = !((col == 3) || (row == 2 || row == 5));
                        break;
                    case 9:
                        Vertical = 2;
                        Horizontal = 2;
                        Condition = !((row == 3 || row == 7) || (col == 3 || col == 7));
                        break;
                    case 12:
                        Vertical = 2;
                        Horizontal = 3;
                        Condition = !((col == 4 || col == 9) || (row == 3 || row == 7 || row == 11));
                        break;
                    case 16:
                        Vertical = 3;
                        Horizontal = 3;
                        Condition = !((row == 4 || row == 9 || row == 14) || (col == 4 || col == 9 || col == 14));
                        break;
                    default:
                        Vertical = 2;
                        Horizontal = 2;
                        Condition = !((row == 3 || row == 7) || (col == 3 || col == 7));
                        break;
                }
            }
        }

        //Determine the current player (Control)
        public void determinePlayer()
        {
            if (currentGame.gameMode == 0)
            {
                currentGame.player = Game.playerType.player1;
            }
            else if (currentGame.gameMode == 1)
            {
                switch (currentGame.player)
                {
                    case Game.playerType.player1: currentGame.player = Game.playerType.player2; break;
                    case Game.playerType.player2: currentGame.player = Game.playerType.player1; break;
                    default: break;
                }
            }
            else
            {
                switch (currentGame.player)
                {
                    case Game.playerType.player1: currentGame.player = Game.playerType.computerAI; break;
                    case Game.playerType.computerAI: currentGame.player = Game.playerType.player1; break;
                    default: break;
                }
            }
        }
        //Displays results of an invalid move (View)
        public void result()
        {
            if (currentGame.gameOn == false)
            {
                switch (currentGame.player)
                {
                    case Game.playerType.player1:
                        if (currentGame.gameMode == 0) { currentGame.message = "You died."; }
                        if (currentGame.gameMode == 1) { currentGame.message = currentGame.playerNames[1] + " wins! Flawless victory!"; }
                        if (currentGame.gameMode == 2) { currentGame.message = "You died."; }
                        break;
                    case Game.playerType.player2: currentGame.message = currentGame.playerNames[0] + " wins! Flawless victory!"; break;
                    case Game.playerType.computerAI: currentGame.message = currentGame.playerNames[0] + " wins! Flawless victory!"; break;
                    default: break;
                }
            }
        }

        private void InitialiseBoard()
        {
            //board.setBoard();
            currentGame.Board.PlayerBoard = new int[9, 9]
            {
               { 4, 0, 0, 0, 1, 0, 0, 0 , 5},
               { 0, 0, 0, 9, 0, 2, 0, 0 , 0},
               { 0, 0, 6, 0, 7, 0, 3, 0 , 0},
               { 0, 5, 0, 3, 0, 4, 0, 1 , 0},
               { 1, 0, 8, 0, 0, 0, 6, 0 , 4},
               { 0, 9, 0, 7, 0, 1, 0, 8 , 0},
               { 0, 0, 5, 0, 4, 0, 1, 0 , 0},
               { 0, 0, 0, 1, 0, 9, 4, 0 , 0},
               { 2, 0, 0, 0,  3, 0, 0, 0, 8}
            };

            //board.PlayerBoard = new int[16, 16]
            //{
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5},
            //   { 4, 0, 0, 0, 1, 0, 0, 0 , 5, 0, 0, 1, 0, 0, 0 , 5}
            //};
            int row = 0; int col = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (currentGame.Board.PlayerBoard[row, col] != 0)
                {
                    buttons[i].Content = $"  {currentGame.Board.PlayerBoard[row, col]}  ";
                }
                else
                {
                    buttons[i].Content = $"     ";
                }
                if (currentGame.Board.PlayerBoard[row, col] != 0)
                    buttons[i].IsEnabled = false;
                if (col == currentGame.Board.BoardSize - 1)
                {
                    row++;
                    col = -1;
                }
                col++;
            }

        }

        private void InitialiseBoardButtons()
        {
            try
            {
                buttons = new Button[currentGame.Board.BoardSize * currentGame.Board.BoardSize];
                counter = 0;
                int row = 0; int col = 0;
                BoardSizeChange bsc = new BoardSizeChange(currentGame.Board, row, col);
                for (row = 0; row < (currentGame.Board.BoardSize + bsc.Horizontal); row++)
                {
                    for (col = 0; col < (currentGame.Board.BoardSize + bsc.Vertical); col++)
                    {
                        bsc = new BoardSizeChange(currentGame.Board, row, col);
                        if (bsc.Condition)
                        {
                            RowDefinition r = new RowDefinition();
                            ColumnDefinition w = new ColumnDefinition();
                            w.Width = GridLength.Auto;
                            r.Height = GridLength.Auto;
                            grid.RowDefinitions.Add(r);
                            grid.ColumnDefinitions.Add(w);
                            button = new Button();
                            Style s = this.FindResource("ButtonStyle1") as Style;
                            button.Style = s;
                            counter++;
                            button.Name = $"btn{counter}";
                            button.FontSize = 24;
                            button.Foreground = Brushes.Black;
                            button.FontWeight = FontWeights.Bold;
                            button.HorizontalContentAlignment = HorizontalAlignment.Center;
                            button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            button.HorizontalAlignment = HorizontalAlignment.Stretch;
                            button.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                            button.Background = Brushes.White;
                            button.BorderBrush = Brushes.Black;
                            button.IsEnabled = true;
                            buttons[counter - 1] = button;
                            Grid.SetRow(button, row);
                            Grid.SetColumn(button, col);
                            grid.Children.Add(button);
                            button.Click += Button_Click;
                            button.MouseWheel += Button_MouseWheel;
                            button.MouseEnter += Button_MouseEnter;
                            button.MouseLeave += Button_MouseLeave;
                            button.MouseRightButtonUp += Button_MouseRightButtonUp;

                        }
                        else
                        {
                            RowDefinition r = new RowDefinition();
                            ColumnDefinition w = new ColumnDefinition();
                            w.Width = GridLength.Auto;
                            r.Height = GridLength.Auto;
                            grid.RowDefinitions.Add(r);
                            grid.ColumnDefinitions.Add(w);

                            Label l = new Label();
                            l.IsEnabled = false;
                            l.Background = Brushes.DarkSlateGray;
                            Grid.SetRow(l, row);
                            Grid.SetColumn(l, col);
                            grid.Children.Add(l);
                        }
                    }
                }

                InitialiseBoard();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Sorry an Error has Occurred, please restart the game.");
            }
        }

        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button b = (Button)sender;
            currentGame.row = (int)mapButtonToArrayValue(b).X;
            currentGame.col = (int)mapButtonToArrayValue(b).Y;

            if (currentGame.Board.PlayerBoard[currentGame.row, currentGame.col] != 0)
            {
                currentGame.Board.PlayerBoard[currentGame.row, currentGame.col] = 0;
                b.Content = "";
            }

        }

        private Point mapButtonToArrayValue(Button b)
        {
            int row = 0; int col = 0;
            for (int i = 1; i <= currentGame.Board.BoardSize * currentGame.Board.BoardSize; i++)
            {
                if (b.Name == ("btn" + (i).ToString()))
                {
                    break;
                }

                if (col == currentGame.Board.BoardSize - 1)
                {
                    row++;
                    col = -1;
                }
                col++;
            }
            return new Point(row, col);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            int row = (int)mapButtonToArrayValue(b).X;
            int col = (int)mapButtonToArrayValue(b).Y;
            int value = currentGame.Board.PlayerBoard[row, col];

            if (value == 0)
            {
                b.Content = "";
            }
            minSelect = 1;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            int row = (int)mapButtonToArrayValue(b).X;
            int col = (int)mapButtonToArrayValue(b).Y;
            int value = currentGame.Board.PlayerBoard[row, col];
            if (value == 0)
            {
                b.Foreground = Brushes.Gray;
                b.Content = $"  {minSelect}  ";
            }
        }

        private void Button_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Button b = (Button)sender;
            int row = (int)mapButtonToArrayValue(b).X;
            int col = (int)mapButtonToArrayValue(b).Y;
            int value = currentGame.Board.PlayerBoard[row, col];
            if (value == 0)
            {
                if (e.Delta > 0 && minSelect < currentGame.Board.BoardSize)
                {
                    minSelect++;
                    b.Content = $"  {minSelect}  ";
                }
                else if (e.Delta < 0 && minSelect > 1)
                {
                    minSelect--;
                    b.Content = $"  {minSelect}  ";
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            try
            {
             updateGame(b);
            }
            catch(Exception RDE)
            {
                txtOutput.Content = RDE.Message;
            }
            minSelect = 1;
        }

        void runGame()
        {
            if (MessageBox.Show("Continue From Last Game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    loadGame();
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("There is no saved game, please start a new game and save.");
                    initGame();
                }
            }
            else
            {
                initGame();
            }
            currentPlayer = player1;
        }

        private void initGame()
        {
            txtOutput.Content = "Enter the details:";
        }

        private void loadGame()
        {
            r = new StreamReader("File");
            bool read = true;
            bool successfulRead = false;
            int linesRead = 0;

            while (read)
            {
                string temp = r.ReadLine();
                linesRead++;
                if (temp == null && linesRead <= 1)
                {
                    read = false;
                    MessageBox.Show("Sorry there are no saved games availaible.", "Noughts and Crosses", MessageBoxButton.OK, MessageBoxImage.Information);
                    initGame();
                    break;
                }
                else if (temp != null)
                {
                    string[] arr = temp.Split(' ');

                    switch (arr[0])
                    {
                        case "b":
                            currentGame.Board = new Board(Convert.ToInt32(arr[1]), Convert.ToChar(arr[2]));
                            break;
                        case "p1":
                            player1 = new Player(arr[1], Convert.ToChar(arr[2]), Convert.ToDouble(arr[3]));
                            break;
                        case "p2":
                            player2 = new Player(arr[1], Convert.ToChar(arr[2]), Convert.ToDouble(arr[3]));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    read = false;
                    successfulRead = true;
                }
            }
            r.Close();
            if (successfulRead)
            {
                InitialiseBoardButtons();
                txtOutput.Content = $"{player1.ToString()},  {player2.ToString()}";
            }
        }

        private void storeGame(Board b, Player p1, Player p2)
        {
            w = new StreamWriter("File");
            if (currentGame.Board != null && p1 != null && p2 != null)
            {
                w.WriteLine($"b {b.BoardSize} {b.BoardStyle}");
                w.WriteLine($"p1 {p1.Name} {p1.Token} {p1.Score}");
                w.WriteLine($"p2 {p2.Name} {p2.Token} {p2.Score}");
            }
            w.Close();
        }

       
        private void updateGame(Button b)
        {


            currentGame.row = (int)mapButtonToArrayValue(b).X;
            currentGame.col = (int)mapButtonToArrayValue(b).Y;

            int CheckWon = 0;

            for (int ii = 0; ii < currentGame.Board.BoardSize; ii++)
            {
                for (int y = 0; y < currentGame.Board.BoardSize; y++)
                {
                    if (currentGame.Board.PlayerBoard[ii, y] == 0)
                    {
                        CheckWon++;
                    }
                }
            }
            if (CheckWon == 0)
            {
                Console.WriteLine("Winner Winner Chicken Dinner!");
            }
            else
            {

                //--------------------------int gameon = 1;

                determinePlayer();
                if (currentGame.player == Game.playerType.computerAI)
                {
                    //currentGame.move();
                }
                else
                {
                    currentGame.check(minSelect);
                }
                result();


                //if (error != 0)
                //{
                //    b.Content = "";
                //    if (error == 1)
                //    {
                //        txtOutput.Content = $"Illegal move! - Move out of bounds!";
                //    }
                //    if (error == 2)
                //    {
                //        txtOutput.Content = $"Illegal move! - Invalid number played!";
                //    }
                //    if (error == 3)
                //    {
                //        txtOutput.Content = $"Illegal move! - Block occupied!";
                //    }
                //    if (error == 4)
                //    {
                //        txtOutput.Content = $"Illegal move! - Duplicate Value in row!";
                //    }
                //    if (error == 5)
                //    {
                //        txtOutput.Content = $"Illegal move! - Duplicate Value in column!";
                //    }
                //    if (error == 6)
                //    {
                //        txtOutput.Content = $"Illegal move! - Duplicate Value in block!";
                //    }
                //}

                //if (error == 0)
                //{
                txtOutput.Content = "Valid Move!";
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                currentGame.Board.PlayerBoard[currentGame.row, currentGame.col] = minSelect;
                b.Foreground = Brushes.Black;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
               // }

            }
        }


        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currentGame.Board.ToString());
        }
    }
}
