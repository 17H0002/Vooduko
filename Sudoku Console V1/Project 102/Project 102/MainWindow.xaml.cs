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

namespace Project_102
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] GameArray = new int[9, 9];
        int BlockLength = 3;
        int BlockHeight = 3;
        int BoardLength = 9;
        int BoardHeight = 9;
        int MoveRow = 0;
        int MoveCol = 0;
        int move = 0;
        int Magic1 = (1 + (3 * 3));


        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < BoardLength; i++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    GameArray[i, y] = 0;
                }
            }
            //test board
            GameArray[0, 1] = 7;
            GameArray[0, 2] = 5;
            GameArray[0, 4] = 9;
            GameArray[0, 8] = 6;
            GameArray[1, 1] = 2;
            GameArray[1, 2] = 3;
            GameArray[1, 4] = 8;
            GameArray[1, 7] = 4;
            GameArray[2, 0] = 8;
            GameArray[2, 5] = 3;
            GameArray[2, 8] = 1;

            GameArray[3, 0] = 5;
            GameArray[3, 3] = 7;
            GameArray[3, 5] = 2;
            GameArray[4, 1] = 4;
            GameArray[4, 3] = 8;
            GameArray[4, 5] = 6;
            GameArray[4, 7] = 2;
            GameArray[5, 3] = 9;
            GameArray[5, 5] = 1;
            GameArray[5, 8] = 3;

            GameArray[6, 0] = 9;
            GameArray[6, 3] = 4;
            GameArray[6, 8] = 7;
            GameArray[7, 1] = 6;
            GameArray[7, 4] = 7;
            GameArray[7, 6] = 5;
            GameArray[7, 7] = 8;
            GameArray[8, 0] = 7;
            GameArray[8, 4] = 1;
            GameArray[8, 6] = 3;
            GameArray[8, 7] = 9;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Print top line
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
            Console.Write("||");
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Print the middle
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int x = 0; x < 9; x++)
            {
                for (int z = 0; z < 9; z++)
                {
                    if ((z + 3 + 1) % 3 == 0)
                    {
                        if (GameArray[x, z] == 0)
                        {
                            Console.Write("  " + "||");
                        }
                        else
                        {
                            if (GameArray[x, z] > 9)
                            {
                                Console.Write(GameArray[x, z] + "||");
                            }
                            else
                            {
                                Console.Write(" " + GameArray[x, z] + "||");
                            }
                        }
                    }
                    else
                    {
                        if (GameArray[x, z] == 0)
                        {
                            Console.Write("  " + "|");
                        }
                        else
                        {
                            if (GameArray[x, z] > 9)
                            {
                                Console.Write(GameArray[x, z] + "|");
                            }
                            else
                            {
                                Console.Write(" " + GameArray[x, z] + "|");
                            }
                        }
                    }
                }

                Console.WriteLine();
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Print Out bottom line
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if ((x + 3 + 1) % 3 == 0)
                {
                    for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
                    {
                        Console.Write("=");
                    }
                }
                else
                {
                    for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
                if (x < (9 - 1))
                {
                    Console.Write("||");
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // End of print board
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }





        private void PlayMoveBut_Click(object sender, RoutedEventArgs e)
        {

            int CheckWon = 0;

            for (int i = 0; i < BoardLength; i++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    if (GameArray[i, y] == 0)
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


                while (gameon == 1 && P1moved == 0)
                {


                    int valid = 0;
                    int error = 0;

                    //Human move

                    MoveRow = Convert.ToInt32(RowText.Text) - 1;
                    MoveCol = Convert.ToInt32(ColText.Text) - 1;
                    move = Convert.ToInt32(MoveText.Text);

                    RowText.Text = "";
                    ColText.Text = "";
                    MoveText.Text = "";

                    if (MoveRow < 0 || MoveRow > 8 || MoveCol < 0 || MoveCol > 8)
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
                    if (GameArray[MoveRow, MoveCol] == 0 && valid == 0)
                    {
                        valid = 1;
                        GameArray[MoveRow, MoveCol] = move;
                        P1moved = 1;

                    }
                    if ((GameArray[MoveRow, MoveCol] != 0) && valid == 0)
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
                            if (GameArray[MoveRow, tt] == move)
                            {
                                dupcnt = dupcnt + 1;

                            }
                        }
                        if (dupcnt == 2)
                        {

                            GameArray[MoveRow, MoveCol] = 0;
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
                            if (GameArray[tt, MoveCol] == move)
                            {
                                dupcnt = dupcnt + 1;
                            }
                        }
                        if (dupcnt == 2)
                        {
                            error = 5;
                            GameArray[MoveRow, MoveCol] = 0;
                            valid = 0;
                            gameon = 0;
                        }
                    }

                    // Check 4 - Duplicate in the box

                    if (valid == 1)
                    {
                        int RBound = (MoveRow / BlockLength) * BlockLength;
                        int CBound = (MoveCol / BlockHeight) * BlockHeight;
                        int[] bline;
                        int ic = 0;
                        int bi = BlockLength * BlockHeight;
                        bline = new int[bi];

                        for (int rc = 0; rc < BlockLength; rc++)
                        {
                            for (int cc = 0; cc < BlockHeight; cc++)
                            {
                                bline[ic] = GameArray[RBound, CBound];
                                ic = ic + 1;
                                CBound = CBound + 1;

                            }
                            RBound = RBound + 1;
                            CBound = (MoveCol / BlockHeight) * BlockHeight;
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
                            GameArray[MoveRow, MoveCol] = 0;
                            valid = 0;
                            gameon = 0;
                        }
                    }

                    if (error != 0)
                    {

                        if (error == 1)
                        {
                            Term.Text = $"Illegal move! - Move out of bounds!";
                        }
                        if (error == 2)
                        {
                            Term.Text = $"Illegal move! - Invalid number played!";
                        }
                        if (error == 3)
                        {
                            Term.Text = $"Illegal move! - Block occupied!";
                        }
                        if (error == 4)
                        {
                            Term.Text = $"Illegal move! - Duplicate Value in row!";
                        }
                        if (error == 5)
                        {
                            Term.Text = $"Illegal move! - Duplicate Value in column!";
                        }
                        if (error == 6)
                        {
                            Term.Text = $"Illegal move! - Duplicate Value in block!";
                        }

                        ColText.IsEnabled = false;
                        RowText.IsEnabled = false;
                        MoveText.IsEnabled = false;
                        PlayMoveBut.IsEnabled = false;

                    }

                    if (error == 0)
                    {
                        Term.Text = "Valid Move!";
                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Print top line
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();
                    Console.Write("||");
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Print the middle
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    for (int x = 0; x < 9; x++)
                    {
                        for (int z = 0; z < 9; z++)
                        {
                            if ((z + 3 + 1) % 3 == 0)
                            {
                                if (GameArray[x, z] == 0)
                                {
                                    Console.Write("  " + "||");
                                }
                                else
                                {
                                    if (GameArray[x, z] > 9)
                                    {
                                        Console.Write(GameArray[x, z] + "||");
                                    }
                                    else
                                    {
                                        Console.Write(" " + GameArray[x, z] + "||");
                                    }
                                }
                            }
                            else
                            {
                                if (GameArray[x, z] == 0)
                                {
                                    Console.Write("  " + "|");
                                }
                                else
                                {
                                    if (GameArray[x, z] > 9)
                                    {
                                        Console.Write(GameArray[x, z] + "|");
                                    }
                                    else
                                    {
                                        Console.Write(" " + GameArray[x, z] + "|");
                                    }
                                }
                            }
                        }

                        Console.WriteLine();
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        // Print Out bottom line
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if ((x + 3 + 1) % 3 == 0)
                        {
                            for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
                            {
                                Console.Write("=");
                            }
                        }
                        else
                        {
                            for (int xx = 0; xx < ((Magic1 * 3) + 2); xx++)
                            {
                                Console.Write("-");
                            }
                        }
                        Console.WriteLine();
                        if (x < (9 - 1))
                        {
                            Console.Write("||");
                        }
                    }
                    Console.WriteLine($"{player} plays {move} in row {MoveRow + 1}, col {MoveCol + 1}");
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // End of print board
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                }
            }



        }
    }
}

