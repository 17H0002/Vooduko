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
using WMPLib;
using System.IO;

namespace SudukoGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        WindowsMediaPlayer soundFX = new WindowsMediaPlayer();
        List<Board> boards = new List<Board>() { };
        Dictionary<string, Game> games = new Dictionary<string, Game>();
        List<string> oldGames;
        Game currentGame;
        private Button button;
        private Button[] buttons;
        private int counter;
        private int minSelect = 1;
       
        public MainWindow()
        {
            InitializeComponent();
            oldGames = new List<string>() { };
            try
            {
                loadFiles();
            }
            catch (FileNotFoundException)
            {
                txtOutput.Content = ("There is no saved game, please start a new game and save.");
            }
            catch (FileFormatException)
            {
                MessageBox.Show("The saved file is empty or corrupted, please start a new game.");
            }
            listViewOldGames.ItemsSource = oldGames;
            player.settings.volume = 50;
            for (int plays = 0; plays < 1000; plays++)
            {
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
               player.URL = "Famitracker - Spooky Scary Skeletons (8-Bit VRC6 Remix).mp3";
            }
        }

        public class BlankPlayerNameException : Exception
        {
            public BlankPlayerNameException()
            {

            }
        }
        public class BlankGameNameException : Exception
        {
            public BlankGameNameException()
            {

            }
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

        private Board chooseBoard(string size)
        {
            int[,] x4 = new int[4, 4]
            {
               { 1, 0, 0, 4},
               { 0, 2, 1, 0},
               { 3, 0, 0, 2},
               { 2, 0, 3, 0}
            };

            //solution
            //   { 1, 3, 2, 4},
            //   { 4, 2, 1, 3},
            //   { 3, 1, 4, 2},
            //   { 2, 4, 3, 1}

            int[,] x9 = new int[9, 9]
            {
               { 0, 0, 7, 9, 0, 5, 4, 1, 0},
               { 3, 0, 5, 2, 0, 0, 9, 8, 7},
               { 9, 0, 1, 8, 7, 4, 6, 5, 0},
               { 0, 3, 4, 1, 5, 0, 0, 2, 6},
               { 5, 9, 2, 7, 0, 0, 0, 3, 4},
               { 0, 8, 6, 3, 4, 2, 0, 0, 9},
               { 4, 5, 3, 0, 0, 0, 7, 0, 8},
               { 2, 1, 0, 4, 9, 7, 3, 6, 5},
               { 6, 0, 0, 5, 8, 3, 2, 0, 1}
            };

            //solution
            //   { 8, 6, 7, 9, 3, 5, 4, 1, 2},
            //   { 3, 4, 5, 2, 1, 6, 9, 8, 7},
            //   { 9, 2, 1, 8, 7, 4, 6, 5, 3},
            //   { 7, 3, 4, 1, 5, 9, 8, 2, 6},
            //   { 5, 9, 2, 7, 6, 8, 1, 3, 4},
            //   { 1, 8, 6, 3, 4, 2, 5, 7, 9},
            //   { 4, 5, 3, 6, 2, 1, 7, 9, 8},
            //   { 2, 1, 8, 4, 9, 7, 3, 6, 5},
            //   { 6, 7, 9, 5, 8, 3, 2, 4, 1}

            int[,] x6 = new int[6, 6]
            {
                { 4, 5, 0, 0, 0, 0},
                { 3, 0, 0, 0, 0, 6},
                { 0, 4, 0, 1, 0, 0},
                { 0, 0, 1, 0, 5, 0},
                { 2, 0, 0, 0, 0, 1},
                { 0, 0, 0, 0, 2, 5}
            };

            //solution
            //    { 4, 5, 6, 2, 1, 3},
            //    { 3, 1, 2, 5, 4, 6},
            //    { 5, 4, 3, 1, 6, 2},
            //    { 6, 2, 1, 3, 5, 4},
            //    { 2, 6, 5, 4, 3, 1},
            //    { 1, 3, 4, 6, 2, 5}

            int[,] x12 = new int[12, 12]
            {
                { 1, 2, 0, 0, 5, 0, 0, 0 , 9, 10, 11, 12 },
                { 0, 12, 9, 10, 1, 2, 0, 4 , 5, 0, 0, 8 },
                { 5, 6, 7, 8, 0, 11, 0, 0 , 1, 2, 3, 4 },
                { 0, 1, 2, 3, 8, 0, 12, 11, 0, 0, 10, 0 },
                { 12, 0, 10, 0, 4, 0, 2, 1, 3, 11, 8, 6 },
                { 4, 0, 8, 11, 0, 9, 10, 6, 2, 0, 0, 5 },
                { 0, 0, 1, 0, 0, 8, 0, 10 , 0, 0, 9, 3 },
                { 0, 0, 11, 12, 0, 4, 0, 2, 8, 0, 0, 7 },
                { 9, 0, 4, 6, 11, 3, 5, 7, 10, 12, 0, 1 },
                { 0, 4, 5, 0, 7, 0, 11, 3, 0, 8, 1, 10 },
                { 0, 11, 12, 1, 0, 0, 4, 9, 7, 3, 0, 0 },
                { 0, 0, 0, 7, 2, 0, 0, 5, 12, 9, 0, 11 }
            };

            //solution
            //{ 1, 2, 3, 4, 5, 6, 7, 8 , 9, 10, 11, 12 },
            //{ 11, 12, 9, 10, 1, 2, 3, 4 , 5, 6, 7, 8 },
            //{ 5, 6, 7, 8, 10, 11, 9, 12 , 1, 2, 3, 4 },
            //{ 6, 1, 2, 3, 8, 5, 12, 11, 4, 7, 10, 9 },
            //{ 12, 9, 10, 5, 4, 7, 2, 1, 3, 11, 8, 6 },
            //{ 4, 7, 8, 11, 3, 9, 10, 6, 2, 1, 12, 5 },
            //{ 7, 5, 1, 2, 12, 8, 6, 10 , 11, 4, 9, 3 },
            //{ 10, 3, 11, 12, 9, 4, 1, 2, 8, 5, 6, 7 },
            //{ 9, 8, 4, 6, 11, 3, 5, 7, 10, 12, 2, 1 },
            //{ 2, 4, 5, 9, 7, 12, 11, 3, 6, 8, 1, 10 },
            //{ 8, 11, 12, 1, 6, 10, 4, 9, 7, 3, 5, 2 },
            //{ 3, 10, 6, 7, 2, 1, 8, 5, 12, 9, 4, 11 }

            int[,] x16 = new int[16, 16]
            {
               { 0, 0, 5, 0, 4, 8, 0, 0, 0, 0, 13, 12, 0, 2, 0, 0},
               { 0, 0, 0, 0, 3, 5, 15, 14, 11, 2, 10, 1, 0, 0, 0 , 0},
               { 1, 0, 4, 6, 0, 0, 0, 0, 0, 0, 0, 0, 8, 7, 0, 10},
               { 0, 0, 2, 3, 0, 10, 0, 0, 0, 0, 5, 0, 11, 15, 0, 0},
               { 2, 12, 0, 0, 0, 0, 6, 11, 10, 15, 0, 0, 0, 0, 4 , 16},
               { 6, 3, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 10 , 7},
               { 0, 10, 0, 0, 9, 0, 2, 8, 3, 1, 0, 7, 0, 0, 12 , 0},
               { 0, 4, 0, 0, 10, 0, 3, 0, 0, 16, 0, 6, 0, 0, 8, 0},
               { 0, 5, 0, 0, 6, 0, 1, 0 , 0, 11, 0, 15, 0, 0, 16 , 0},
               { 0, 6, 0, 0, 12, 0, 8, 16 , 5, 4, 0, 9, 0, 0, 2 , 0},
               { 16, 9, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 13, 0, 11, 14},
               { 10, 15, 0, 0, 0, 0, 14, 3 , 16, 13, 0, 0, 0, 0, 1 , 6},
               { 0, 0, 6, 7, 0, 3, 0, 0, 0, 0, 11, 0, 14, 8, 0, 0},
               { 12, 0, 3, 16, 0, 0, 0, 0, 0, 0, 0, 0, 2, 4, 0, 5},
               { 0, 0, 0, 0, 14, 15, 13, 12 , 8, 7, 9, 3, 0, 0, 0, 0},
               { 0, 0, 10, 0, 8, 6, 0, 0, 0, 0, 1, 4, 0, 13, 0, 0}
            };

            //solution
            //   { 15, 16, 5, 10, 4, 8, 11, 6, 7, 9, 13, 12, 1, 2, 14, 3},
            //   { 8, 7, 12, 9, 3, 5, 15, 14, 11, 2, 10, 1, 16, 6, 13 , 4},
            //   { 1, 11, 4, 6, 2, 12, 9, 13, 15, 3, 16, 14, 8, 7, 5, 10},
            //   { 14, 13, 2, 3, 16, 10, 7, 1, 4, 6, 5, 8, 11, 15, 9, 12},
            //   { 2, 12, 9, 8, 13, 7, 6, 11, 10, 15, 14, 5, 3, 1, 4 , 16},
            //   { 6, 3, 16, 14, 15, 1, 12, 4, 13, 8, 2, 11, 9, 5, 10 , 7},
            //   { 5, 10, 15, 11, 9, 16, 2, 8, 3, 1, 4, 7, 6, 14, 12 , 13},
            //   { 7, 4, 1, 13, 10, 14, 3, 5, 9, 16, 12, 6, 15, 11, 8, 2},
            //   { 3, 5, 13, 12, 6, 2, 1, 10 , 14, 11, 7, 15, 4, 9, 16 , 8},
            //   { 11, 6, 14, 1, 12, 13, 8, 16 , 5, 4, 3, 9, 7, 10, 2 , 15},
            //   { 16, 9, 8, 2, 7, 4, 5, 15, 1, 12, 6, 10, 13, 3, 11, 14},
            //   { 10, 15, 7, 4, 11, 9, 14, 3 , 16, 13, 8, 2, 5, 12, 1 , 6},
            //   { 13, 1, 6, 7, 5, 3, 4, 2, 12, 10, 11, 16, 14, 8, 15, 9},
            //   { 12, 8, 3, 16, 1, 11, 10, 9, 6, 14, 15, 13, 2, 4, 7 , 5},
            //   { 4, 2, 11, 5, 14, 15, 13, 12 , 8, 7, 9, 3, 10, 16, 6, 1},
            //   { 9, 14, 10, 15, 8, 6, 16, 7, 2, 5, 1, 4, 12, 13, 3, 11}

            Board b;
            switch (size)
            {
                case "4x4": b = new Board(x4); break;
                case "6x6": b = new Board(x6); break;
                case "9x9": b = new Board(x9); break;
                case "12x12": b = new Board(x12); break;
                case "16x16": b = new Board(x16); break;
                default: b = new Board(x9); break;
            }
            return b;
        }

        private void InitialiseBoard()
        {
          
            int row = 0; int col = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (currentGame.Board.PlayerBoard[row, col] != 0)
                {
                    buttons[i].Content = $"  {currentGame.Board.PlayerBoard[row, col]}  ";
                }
                else
                {
                    buttons[i].Content = $"      ";
                }
                if (currentGame.Board.PlayerBoard[row, col] != 0 && !currentGame.ValidMoves.ContainsKey(new Point(row, col)))
                {
                    buttons[i].IsEnabled = false;
                }else
                {
                    buttons[i].Foreground = Brushes.Blue;
                }
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
                            l.Background = Brushes.DarkOrange;
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
                currentGame.ValidMoves.Remove(new Point(currentGame.row, currentGame.col));
                currentGame.numMoves++;
                updateCount();
                save(false);
            }

        }

        public void updateCount()
        {
            countLbl.Content = $"Moves: {currentGame.numMoves}";
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
                b.Content = $"     ";
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
            currentGame.numMoves++;
            currentGame.gameOn = false;
            updateCount();
            save(false);
        }

        private void initGame()
        {
            string sze = "";
            switch (comboBox.SelectedIndex)
            {
                case 0: sze = "4x4"; break; 
                case 1: sze = "6x6"; break;
                case 2: sze = "9x9"; break;
                case 3: sze = "12x12"; break;
                case 4: sze = "16x16"; break;
            }      

            try
            {
                if (txtGameName.Text == "")
                {
                    throw new BlankGameNameException();
                }
                else
                {
                    if (txtPlayer1name.Text == "")
                    {
                        throw new BlankPlayerNameException();
                    }
                    else
                    {
                        int i = 0;
                        if(radGameMode0.IsPressed == true)
                        {
                            i = 0;
                        }
                        else if(radGameMode1.IsPressed == true)
                        {
                            i = 1;
                        }
                        if (radGameMode2.IsPressed == true)
                        {
                            i = 2;
                        }
                        currentGame = new Game(txtGameName.Text, new string[2] { txtPlayer1name.Text, txtPlayer2name.Text }, 0, chooseBoard(sze), i, false, 0, "", 0, 0, 0);
                        InitialiseBoardButtons();
                        games.Add(currentGame.gameName, currentGame);
                        soundFX.URL = "126422__cabeeno-rossley__level-up.WAV";
                        Details.Visibility = Visibility.Hidden;
                        grid.Visibility = Visibility.Visible;
                        countLbl.Visibility = Visibility.Visible;
                        txtOutput.Content = "Play a move!";

                    }
                }

            }
            catch(BlankPlayerNameException)
            {
                MessageBox.Show("You need a name buddy!");
            }
            catch(BlankGameNameException)
            {
                MessageBox.Show("You need to name your game buddy!");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("A saved game with this name already exists!");
            }
        }

        public void save(bool delete)
        {
            StreamWriter files;
            StreamWriter w;
            files = new StreamWriter("Games");

            foreach (KeyValuePair<string, Game> g in games)
            {
                if (!delete)
                {
                    w = new StreamWriter($"{g.Value.gameName}");
                    w.WriteLine(g.Value.gameName + "," + g.Value.playerNames[0] + "," + g.Value.playerNames[1] + "," + g.Value.player + "," + g.Value.Board.BoardSize + "," + g.Value.Board.ToElements() + "," + Convert.ToString(g.Value.gameMode) + "," + Convert.ToString(g.Value.gameOn) + "," + Convert.ToInt32(g.Value.numMoves) + "," + g.Value.message + "," + Convert.ToInt32(g.Value.row) + "," + Convert.ToInt32(g.Value.col) + "," + Convert.ToInt32(g.Value.move));
                    foreach (KeyValuePair<Point, int> k in g.Value.ValidMoves)
                    {
                        w.WriteLine($"$EVS$,{k.Key.X},{k.Key.Y},{k.Value}");
                    }
                    w.Close();
                }
                files.WriteLine($"{g.Value.gameName}");
            }
            files.Close();
        }

        public void load(string gameToLoad)
        {
            string[] lines = File.ReadAllLines(gameToLoad);
            if (lines.Length != 0)
            {
                foreach (string line in lines)
                {
                    if (line != null && line != "")
                    {
                        string[] splitter = line.Split(',');
                        if (splitter[0] != "$EVS$")
                        {
                            Board boarder = new Board(Convert.ToInt32(splitter[4]));
                            string[] rows = splitter[5].Split('/');
                            for (int row = 0; row < boarder.BoardSize; row++)
                            {
                                string[] elements = rows[row].Split('-');
                                for (int col = 0; col < boarder.BoardSize; col++)
                                {
                                    boarder.PlayerBoard[row, col] = Convert.ToInt32(elements[col]);
                                }
                            }
                            string[] loadPlayers = new string[2] { splitter[1], splitter[2] };
                            int playerKey = 0;
                            switch (splitter[3])
                            {
                                case "player1": playerKey = 0; break;
                                case "player2": playerKey = 1; break;
                                case "computerAI": playerKey = 2; break;
                                default: break;
                            }
                            currentGame = new Game(splitter[0], loadPlayers, playerKey, boarder, Convert.ToInt32(splitter[6]), Convert.ToBoolean(splitter[7]), Convert.ToInt32(splitter[8]), splitter[9], Convert.ToInt32(splitter[10]), Convert.ToInt32(splitter[11]), Convert.ToInt32(splitter[12]));
                            games.Add(currentGame.gameName, currentGame);
                        }
                        else
                        {
                            currentGame.ValidMoves.Add(new Point(Convert.ToInt32(splitter[1]), Convert.ToInt32(splitter[2])), Convert.ToInt32(splitter[3]));
                        }
                    }
                }
                
            }
            else
            {
                throw new FileFormatException();
            }
        }

        private void loadFiles()
        {
            string[] lines = File.ReadAllLines("Games");
            if (lines.Length != 0)
            {
                foreach (string line in lines)
                {
                    if (line != null && line != "")
                    {
                        string[] splitter = line.Split(',');
                        oldGames.Add(splitter[0]);
                        load(line);
                    }
                }
            }
        }
       
        private void updateGame(Button b)
        {

            currentGame.row = (int)mapButtonToArrayValue(b).X;
            currentGame.col = (int)mapButtonToArrayValue(b).Y;

            int CheckWon = 0;


                determinePlayer();
                if (currentGame.player == Game.playerType.computerAI)
                {
                    //currentGame.move();
                }
                else
                {
                    currentGame.check(minSelect);
                   

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
                        MessageBox.Show("Winner Winner Chicken Dinner!");
                    }
                }
                result();
                txtOutput.Content = "Valid Move!";
                currentGame.Board.PlayerBoard[currentGame.row, currentGame.col] = minSelect;
                b.Foreground = Brushes.Blue;
                currentGame.ValidMoves.Add(new Point(currentGame.row, currentGame.col), minSelect);
            
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(currentGame.Board.ToString());
            save(false);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {

            initGame();          
        }

        private void btnDeleteGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(oldGames[listViewOldGames.SelectedIndex]);
                games.Remove(oldGames[listViewOldGames.SelectedIndex]);
                oldGames.RemoveAt(listViewOldGames.SelectedIndex);
                listViewOldGames.SelectedIndex = -1;
                save(true);
                listViewOldGames.Items.Refresh();
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to select a game to delete.");
            }
        }

        private void btnResumeGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentGame = games[oldGames[listViewOldGames.SelectedIndex]];
                InitialiseBoardButtons();
                Details.Visibility = Visibility.Hidden;
                grid.Visibility = Visibility.Visible;
                countLbl.Visibility = Visibility.Visible;
                txtOutput.Content = "Make your move!";
                save(false);
                updateCount();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("You need to select a game to resume.");
            }
            catch (FileNotFoundException)
            {

                MessageBox.Show("There is no saved game, please start a new game and save.");
                Details.Visibility = Visibility.Visible;
                grid.Visibility = Visibility.Hidden;
                oldGames.Clear();
                File.CreateText("Games");

            }
            catch (FileFormatException)
            {
                MessageBox.Show("The saved file is empty or corrupted, please start a new game.");
                Details.Visibility = Visibility.Visible;
                grid.Visibility = Visibility.Hidden;
                foreach (string s in oldGames)
                {
                    File.Delete(s);
                }
                oldGames.Clear();
                File.CreateText("Games");
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("The saved file is empty or corrupted, please start a new game.");
                Details.Visibility = Visibility.Visible;
                grid.Visibility = Visibility.Hidden;
                foreach (string s in oldGames)
                {
                    File.Delete(s);
                }
                oldGames.Clear();
                File.CreateText("Games");
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("The saved file is empty or corrupted, please start a new game.");
                Details.Visibility = Visibility.Visible;
                grid.Visibility = Visibility.Hidden;
                foreach (string s in oldGames)
                {
                    File.Delete(s);
                }
                oldGames.Clear();
                File.CreateText("Games");
            }
        }
        Board generateAlternativeBoard(Board b, int n)
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                int rowRnd = rnd.Next(1, b.BoardSize + 1);
                int colRnd = rnd.Next(1, b.BoardSize + 1);
                int valueRnd = rnd.Next(1, 10);
                b.PlayerBoard[rowRnd, colRnd] = valueRnd;
            }
            return b;
        }
    }
}