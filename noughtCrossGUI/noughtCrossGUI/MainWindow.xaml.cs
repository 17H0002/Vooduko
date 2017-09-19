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

namespace noughtCrossGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StreamWriter w;
        private StreamReader r;
        private Board board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private bool game = true;
        private int turnCount = 0;
        private Button button;
        private int counter;
        private Button[] buttons;


        public MainWindow()
        {
            InitializeComponent();
            runGame();
            //initGrid.Visibility = Visibility.Hidden;
            //initGrid.IsEnabled = false;
            //board = new Board(9, ' ');
            //player1 = new Player("Andrew", 'X');
            //player2 = new Player("Shann", 'O');
            //InitialiseBoardButtons();
        }

        //Noughts and crosses

        private void InitialiseBoardButtons()
        {
            try
            {
                counter = 0;
                for (int row = 0; row < board.BoardSize; row++)
                {
                    for (int col = 0; col < board.BoardSize; col++)
                    {
                        RowDefinition r = new RowDefinition();
                        r.Height = System.Windows.GridLength.Auto;
                        ColumnDefinition w = new ColumnDefinition();
                        w.Width = System.Windows.GridLength.Auto;

                        grid.RowDefinitions.Add(r);
                        grid.ColumnDefinitions.Add(w);
                        counter++;
                        button = new Button();
                        button.Name = $"btn{counter}";
                        button.Content = $"   {board.BoardStyle}   ";
                        button.FontSize = 32;
                        button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                        button.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        button.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                        button.IsEnabled = true;
                        Grid.SetRow(button, row);
                        Grid.SetColumn(button, col);
                        grid.Children.Add(button);
                        button.Click += Button_Click;
                    }
                }
                currentPlayer = player1;
                txtOutput.Content = ($"{currentPlayer.Name}'s turn, choose a position.");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Sorry an Error has Occurred, please restart the game.");
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
            int row = 0; int col = 0;
            for(int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Content = $"   {board.PlayerBoard[row, col]}   ";
                if(board.PlayerBoard[row,col] != ' ')
                    buttons[i].IsEnabled = false;
                if (col == board.BoardSize - 1)
                {
                    row++;
                    col = -1;
                }
                col++;
            }
            MessageBox.Show(board.ToString());
        }

        //private void InitialiseBoardButtons()
        //{
        //    try
        //    {
        //        buttons = new Button[board.BoardSize * board.BoardSize];
        //        counter = 0;
        //        int row = 0; int col = 0;
        //        BoardSizeChange bsc = new BoardSizeChange(board, row, col);
        //        for (row = 0; row < (board.BoardSize + bsc.Horizontal); row++)
        //        {
        //            for (col = 0; col < (board.BoardSize + bsc.Vertical); col++)
        //            {
        //                bsc = new BoardSizeChange(board, row, col);
        //                if (bsc.Condition)
        //                {
        //                    RowDefinition r = new RowDefinition();
        //                    ColumnDefinition w = new ColumnDefinition();
        //                    w.Width = GridLength.Auto;
        //                    r.Height = GridLength.Auto;
        //                    grid.RowDefinitions.Add(r);
        //                    grid.ColumnDefinitions.Add(w);
        //                    button = new Button();
        //                    Style s = this.FindResource("ButtonStyle1") as Style;
        //                    button.Style = s;
        //                    counter++;
        //                    button.Name = $"btn{counter}";
        //                    button.FontSize = 24;
        //                    button.HorizontalContentAlignment = HorizontalAlignment.Center;
        //                    button.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
        //                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
        //                    button.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        //                    button.Background = Brushes.White;
        //                    button.BorderBrush = Brushes.Black;
        //                    button.IsEnabled = true;
        //                    buttons[counter - 1] = button;
        //                    Grid.SetRow(button, row);
        //                    Grid.SetColumn(button, col);
        //                    grid.Children.Add(button);
        //                    button.Click += Button_Click;
        //                }
        //                else
        //                {
        //                    RowDefinition r = new RowDefinition();
        //                    ColumnDefinition w = new ColumnDefinition();
        //                    w.Width = GridLength.Auto;
        //                    r.Height = GridLength.Auto;
        //                    grid.RowDefinitions.Add(r);
        //                    grid.ColumnDefinitions.Add(w);

        //                    Label l = new Label();
        //                    l.IsEnabled = false;
        //                    l.Background = Brushes.DarkSlateGray;
        //                    Grid.SetRow(l, row);
        //                    Grid.SetColumn(l, col);
        //                    grid.Children.Add(l);

        //                }
        //            }
        //        }

        //        InitialiseBoard();
        //        currentPlayer = player1;
        //        txtOutput.Content = ($"{currentPlayer.Name}'s turn, choose a position.");
        //    }
        //    catch (NullReferenceException)
        //    {
        //        MessageBox.Show("Sorry an Error has Occurred, please restart the game.");
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            updateGame(b);
        }

        void runGame()
        {
            game = true;
            if (MessageBox.Show("Continue From Last Game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    initGrid.Visibility = Visibility.Hidden;
                    initGrid.IsEnabled = false;
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
            if (game)
            {
                turn(b);
                if (turnCount >= board.BoardSize * board.BoardSize && !Win(currentPlayer.Token))
                {
                    txtOutput.Content = "It is a draw";
                    game = false;

                    if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to play another game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        board.setBoard();
                        turnCount = 0;
                        game = true;
                        txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                        InitialiseBoardButtons();
                    }
                    else
                    {
                        game = false;
                        if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to save your game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            storeGame(board, player1, player2);

                            if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                this.Close();
                            }
                            else
                            {
                                board.setBoard();
                                turnCount = 0;
                                game = true;
                                txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                InitialiseBoardButtons();
                            }
                        }
                        else
                        {
                            w = new StreamWriter("File");
                            w.Close();

                            if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                this.Close();
                            }
                            else
                            {
                                board.setBoard();
                                turnCount = 0;
                                game = true;
                                txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                InitialiseBoardButtons();
                            }
                        }
                    }

                }
            }
        }

        private void initGame()
        {
            txtOutput.Content = "Enter the details:";
            initGrid.IsEnabled = true;
            initGrid.Visibility = Visibility.Visible;
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

        private bool Win(char token)
        {
            //ROWS
            bool winRows = false;
            int falseCount = 0;
            for (int row = 0; row < board.BoardSize; row++)
            {
                for (int col = 0; col < board.BoardSize; col++)
                {
                    if (board.PlayerBoard[row, col] != token)
                    {
                        falseCount++;
                        break;
                    }
                }
            }
            if (falseCount == board.BoardSize - 1) { winRows = true; }

            //COLS
            bool winCols = false;
            int falseCount2 = 0;
            for (int col = 0; col < board.BoardSize; col++)
            {
                for (int row = 0; row < board.BoardSize; row++)
                {
                    if (board.PlayerBoard[row, col] != token)
                    {
                        falseCount2++;
                        break;
                    }
                }
            }
            if (falseCount2 == board.BoardSize - 1) { winCols = true; }

            //DIAGONALS
            bool winDiag = true;
            for (int i = 0; i < board.BoardSize; i++)
            {
                if (board.PlayerBoard[i, i] != token)
                {
                    winDiag = false;
                    break;
                }
            }

            bool winDiag2 = true;
            for (int i = 0; i < board.BoardSize; i++)
            {
                if (board.PlayerBoard[i, board.BoardSize - 1 - i] != token)
                {
                    winDiag2 = false;
                    break;
                }
            }

            if (winRows || winCols || winDiag || winDiag2)
            {
                game = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void turn(Button b)
        {
            int row = 0; int col = 0;
            for (int i = 1; i <= board.BoardSize * board.BoardSize; i++)
            {
                if (b.Name == ("btn" + (i).ToString()))
                {
                    if (board.PlayerBoard[row, col] == player1.Token || board.PlayerBoard[row, col] == player2.Token)
                    {
                        txtOutput.Content = $"Invalid play {currentPlayer.Name}, please choose an unoccupied space on the board!";
                    }
                    else
                    {
                        turnCount++;
                        board.PlayerBoard[row, col] = currentPlayer.Token;
                        b.Content = $"   {currentPlayer.Token}   ";
                        if (currentPlayer == player1)
                        {
                            if (Win(currentPlayer.Token))
                            {
                                currentPlayer.Score++;
                                txtOutput.Content = $"{currentPlayer.Name} has Won!";

                                if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to play another game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    board.setBoard();
                                    turnCount = 0;
                                    game = true;
                                    txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                    InitialiseBoardButtons();
                                }
                                else
                                {
                                    game = false;
                                    if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to save your game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        storeGame(board, player1, player2);

                                        if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            this.Close();
                                        }
                                        else
                                        {
                                            board.setBoard();
                                            turnCount = 0;
                                            game = true;
                                            txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                            InitialiseBoardButtons();
                                        }
                                    }
                                    else
                                    {
                                        w = new StreamWriter("File");
                                        w.Close();

                                        if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            this.Close();
                                        }
                                        else
                                        {
                                            board.setBoard();
                                            turnCount = 0;
                                            game = true;
                                            txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                            InitialiseBoardButtons();
                                        }
                                    }
                                }

                                break;
                            }
                            else
                            {
                                currentPlayer = player2;
                                txtOutput.Content = ($"{currentPlayer.Name}'s turn, choose a position.");
                            }
                        }
                        else
                        {
                            if (Win(currentPlayer.Token))
                            {
                                currentPlayer.Score++;
                                txtOutput.Content = $"{currentPlayer.Name} has Won!";

                                if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to play another game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    board.setBoard();
                                    turnCount = 0;
                                    game = true;
                                    txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                    InitialiseBoardButtons();
                                }
                                else
                                {
                                    game = false;
                                    if (MessageBox.Show($"{player1.Name} and {player2.Name}, Would you like to save your game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        storeGame(board, player1, player2);

                                        if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            this.Close();
                                        }
                                        else
                                        {
                                            board.setBoard();
                                            turnCount = 0;
                                            game = true;
                                            txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                            InitialiseBoardButtons();
                                        }
                                    }
                                    else
                                    {
                                        w = new StreamWriter("File");
                                        w.Close();

                                        if (MessageBox.Show($"{player1.Name} and {player2.Name}, Are you sure you want to leave?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            this.Close();
                                        }
                                        else
                                        {
                                            board.setBoard();
                                            turnCount = 0;
                                            game = true;
                                            txtOutput.Content = $"Players: {player1.ToString()}, {player2.ToString()}";
                                            InitialiseBoardButtons();
                                        }
                                    }
                                }
                                break;
                            }
                            else
                            {
                                currentPlayer = player1;
                                txtOutput.Content = ($"{currentPlayer.Name}'s turn, choose a position.");
                            }
                        }
                    }
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

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                initGrid.Visibility = Visibility.Hidden;
                player1 = new Player(txtP1Name.Text, Convert.ToChar(txtP1Tkn.Text));
                player2 = new Player(txtP2Name.Text, Convert.ToChar(txtP2Tkn.Text));
                board = new Board(Convert.ToInt32(txtBoardSize.Text), Convert.ToChar(txtBoardStyle.Text));
                currentPlayer = player1;
                InitialiseBoardButtons();
                initGrid.IsEnabled = false;
                txtOutput.Content = ($"{currentPlayer.Name}'s turn, choose a position.");
            }catch(FormatException)
            {
                MessageBox.Show($"Sorry your inputted settings are not supported, please try again.");
                initGame();
            }
        }
    }
}
