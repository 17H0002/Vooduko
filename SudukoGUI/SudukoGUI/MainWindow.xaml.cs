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

        public MainWindow()
        {
            InitializeComponent();
            runGame();
            board = new Board(9, ' ');
            slider.Maximum = board.BoardSize;
            
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
            board.PlayerBoard = new char[9, 9]
            {
               { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
               { ' ', ' ', ' ', '9', ' ', '2', ' ', ' ' , ' '},
               { ' ', ' ', '6', ' ', '7', ' ', '3', ' ' , ' '},
               { ' ', '5', ' ', '3', ' ', '4', ' ', '1' , ' '},
               { '1', ' ', '8', ' ', ' ', ' ', '6', ' ' , '4'},
               { ' ', '9', ' ', '7', ' ', '1', ' ', '8' , ' '},
               { ' ', ' ', '5', ' ', '4', ' ', '1', ' ' , ' '},
               { ' ', ' ', ' ', '1', ' ', '9', '4', ' ' , ' '},
               { '2', ' ', ' ', ' ',  '3', ' ', ' ', ' ', '8'}
            };

            //board.PlayerBoard = new char[16, 16]
            //{
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'},
            //   { '4', ' ', ' ', ' ', '1', ' ', ' ', ' ' , '5', ' ', ' ', '1', ' ', ' ', ' ' , '5'}
            //};
            int row = 0; int col = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Content = $"  {board.PlayerBoard[row, col]}  ";
                if (board.PlayerBoard[row, col] != ' ')
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            updateGame(b);
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

        void updateGame(Button b)
        {
            turn(b);
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

        private void turn(Button b)
        {
            int row = 0; int col = 0;
            for (int i = 1; i <= board.BoardSize * board.BoardSize; i++)
            {
                if (b.Name == ("btn" + (i).ToString()))
                {
                    board.PlayerBoard[row, col] = Convert.ToString(slider.Value)[0];
                    b.Content = Convert.ToString(slider.Value)[0];
                    break;
                }
                if (col == board.BoardSize - 1)
                {
                    row++;
                    col = -1;
                }
                col++;
            }
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(board.ToString());
        }
    }
}
