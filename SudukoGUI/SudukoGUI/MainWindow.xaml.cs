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
        private StreamWriter w;
        private StreamReader r;
        private Board board;
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
            runGame();
            board = new Board(9, ' ');
            InitialiseBoardButtons();
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

        private void InitialiseBoard()
        {
            //board.setBoard();
            board.PlayerBoard = new int[9, 9]
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
                if (board.PlayerBoard[row,col] != 0)
                {
                    buttons[i].Content = $"  {board.PlayerBoard[row, col]}  ";
                }
                else
                {
                    buttons[i].Content = $"     ";
                }
                if (board.PlayerBoard[row, col] != 0)
                    buttons[i].IsEnabled = false;
                if (col == board.BoardSize - 1)
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
                buttons = new Button[board.BoardSize * board.BoardSize];
                counter = 0;
                int row = 0; int col = 0;
                BoardSizeChange bsc = new BoardSizeChange(board, row, col);
                for (row = 0; row < (board.BoardSize + bsc.Horizontal); row++)
                {
                    for (col = 0; col < (board.BoardSize + bsc.Vertical); col++)
                    {
                        bsc = new BoardSizeChange(board, row, col);
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

        private Point mapButtonToArrayValue(Button b)
        {
            int row = 0; int col = 0;
            for (int i = 1; i <= board.BoardSize * board.BoardSize; i++)
            {
                if (b.Name == ("btn" + (i).ToString()))
                {
                    break;
                }

                if (col == board.BoardSize - 1)
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
            int value = board.PlayerBoard[row, col];

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
            int value = board.PlayerBoard[row, col];
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
            int value = board.PlayerBoard[row, col];
            if (value == 0)
            {
                if (e.Delta > 0 && minSelect < board.BoardSize)
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
            updateGame(b);
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
                            board = new Board(Convert.ToInt32(arr[1]), Convert.ToChar(arr[2]));
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
            if (board != null && p1 != null && p2 != null)
            {
                w.WriteLine($"b {b.BoardSize} {b.BoardStyle}");
                w.WriteLine($"p1 {p1.Name} {p1.Token} {p1.Score}");
                w.WriteLine($"p2 {p2.Name} {p2.Token} {p2.Score}");
            }
            w.Close();
        }

        private void updateGame(Button b)
        {

            int row = (int)mapButtonToArrayValue(b).X;
            int col = (int)mapButtonToArrayValue(b).Y;

            int CheckWon = 0;

            for (int ii = 0; ii < board.BoardSize; ii++)
            {
                for (int y = 0; y < board.BoardSize; y++)
                {
                    if (board.PlayerBoard[ii, y] == 0)
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

                int gameon = 1;
                int P1moved = 0;
                string player = "Player 1";

                // Basic game loop control
                if (gameon == 1 && P1moved == 0)
                {
                    int valid = 0;
                    int error = 0;

                    //Human move
                    int move = minSelect;
                    if (row < 0 || row > 8 || row < 0 || row > 8)
                    {
                        valid = 0;
                        gameon = 0;
                        error = 1;


                    }
                    if ((move > 9 || move < 1) && valid == 0)
                    {
                        valid = 0;
                        gameon = 0;
                        error = 2;

                    }
                    if (board.PlayerBoard[row, col] == 0 && valid == 0)
                    {
                        valid = 1;
                        board.PlayerBoard[row, col] = move;
                        P1moved = 1;

                    }
                    if ((board.PlayerBoard[row, col] != 0) && valid == 0)
                    {
                        valid = 0;
                        gameon = 0;
                        error = 3;

                    }

                    // Check 2 - Duplicate in the row?

                    if (valid == 1)
                    {
                        int dupcnt = 0;

                        for (int tt = 0; tt < 9; tt++)
                        {
                            if (board.PlayerBoard[row, tt] == move)
                            {
                                dupcnt = dupcnt + 1;

                            }
                        }
                        if (dupcnt == 2)
                        {

                            board.PlayerBoard[row, col] = 0;
                            error = 4;
                            valid = 0;
                            gameon = 0;
                        }
                    }

                    // Check 3 - Duplicate in the column?

                    if (valid == 1)
                    {
                        int dupcnt = 0;

                        for (int tt = 0; tt < 9; tt++)
                        {
                            if (board.PlayerBoard[tt, col] == move)
                            {
                                dupcnt = dupcnt + 1;
                            }
                        }
                        if (dupcnt == 2)
                        {
                            error = 5;
                            board.PlayerBoard[row, col] = 0;
                            valid = 0;
                            gameon = 0;
                        }
                    }

                    // Check 4 - Duplicate in the box

                    if (valid == 1)
                    {
                        int RBound = (row / board.BlockLength) * board.BlockLength;
                        int CBound = (col / board.BlockHeight) * board.BlockHeight;
                        int[] bline;
                        int ic = 0;
                        int bi = board.BlockLength * board.BlockHeight;
                        bline = new int[bi];

                        for (int rc = 0; rc < board.BlockLength; rc++)
                        {
                            for (int cc = 0; cc < board.BlockHeight; cc++)
                            {
                                bline[ic] = board.PlayerBoard[RBound, CBound];
                                ic = ic + 1;
                                CBound = CBound + 1;

                            }
                            RBound = RBound + 1;
                            CBound = (col / board.BlockHeight) * board.BlockLength;
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
                            error = 6;
                            board.PlayerBoard[row, col] = 0;
                            valid = 0;
                            gameon = 0;
                        }
                    }

                    if (error != 0)
                    {
                        b.Content = "";
                        if (error == 1)
                        {
                            txtOutput.Content = $"Illegal move! - Move out of bounds!";
                        }
                        if (error == 2)
                        {
                            txtOutput.Content = $"Illegal move! - Invalid number played!";
                        }
                        if (error == 3)
                        {
                            txtOutput.Content = $"Illegal move! - Block occupied!";
                        }
                        if (error == 4)
                        {
                            txtOutput.Content = $"Illegal move! - Duplicate Value in row!";
                        }
                        if (error == 5)
                        {
                            txtOutput.Content = $"Illegal move! - Duplicate Value in column!";
                        }
                        if (error == 6)
                        {
                            txtOutput.Content = $"Illegal move! - Duplicate Value in block!";
                        }
                    }

                    if (error == 0)
                    {
                        txtOutput.Content = "Valid Move!";
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        board.PlayerBoard[row, col] = minSelect;
                        b.Foreground = Brushes.Black;
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                }
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(board.ToString());
        }
    }
}
