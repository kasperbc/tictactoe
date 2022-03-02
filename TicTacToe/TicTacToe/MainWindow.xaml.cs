using System;
using System.Diagnostics;
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

namespace TicTacToe
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

        private int clickCount = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            MakeMove(button.Name);
        }

        private void MakeMove(int tile)
        {
            Button? button = FindName("Button" + (tile + 1)) as Button;

            if (button != null)
            {
                // Muuta nappi X tai O vuorotellen
                if (clickCount % 2 == 0)
                {
                    button.Content = "X";
                    button.Foreground = Brushes.Black;
                }
                else
                {
                    button.Content = "O";
                    button.Foreground = Brushes.Red;
                }

                button.IsHitTestVisible = false;
            }

            // Lisää nappien painallusmäärää
            clickCount++;

            CheckForGameEnd();
        }

        private void MakeMove(string buttonName)
        {
            char[] nameCharArray = buttonName.ToCharArray();
            
            if (char.IsDigit(nameCharArray[6]))
            {
                MakeMove(int.Parse(nameCharArray[6].ToString()) - 1);
            }
        }

        /// <summary>
        /// Tarkista, onko joku voittanut tai onko tasapeli
        /// </summary>
        private void CheckForGameEnd()
        {
            // Tarkista jos kukaan on voittanut
            int winner = CheckForWinner();

            // Jos peli päättynyt, kirjoita tulos pelialueeseen
            if (clickCount == 9 || winner != 0)
            {
                TextBlock? resultsText = FindName("Results") as TextBlock;

                if (resultsText is TextBlock)
                {
                    // Rasti voittaa
                    if (winner == 1)
                    {
                        resultsText.Text = "X wins";
                        resultsText.Foreground = Brushes.Black;
                    }
                    // Nolla voittaa
                    else if (winner == 2)
                    {
                        resultsText.Text = "O wins";
                        resultsText.Foreground = Brushes.DarkRed;
                    }
                    // Tasapeli
                    else if (clickCount == 9)
                    {
                        resultsText.Text = "Tie";
                        resultsText.Foreground = Brushes.DarkSlateGray;
                    }

                    // Ottaa kaikki ristinolla-napit pois käytöstä
                    TogglePlayButtons(false);
                }
            }
            // Make random move
            else if (clickCount % 2 != 0)
            {
                Random rng = new Random();
                bool moveSuccesful = false;
                while (!moveSuccesful)
                {
                    moveSuccesful = TryMakeMove(rng.Next(8), true);
                }
            }
        }

        /// <summary>
        /// Tarkistaa jos liikkeen pystyy annetussa ruudussa. Palauttaa true jos liike mahdollinen ja false jos ei.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool TryMakeMove(int tile, bool makeMoveIfPossible)
        {
            string[] grid = GetButtonGrid();

            if (grid[tile].Equals("N"))
            {
                if (makeMoveIfPossible)
                {
                    MakeMove(tile);
                }
                return true;
            }

            return false;
        }
        private bool TryMakeMove(int tile)
        {
            return TryMakeMove(tile, false);
        }

        /// <summary>
        /// Sulje applikaatio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Quit(object sender, RoutedEventArgs e)
        {
            // Sulkee applikaation
            Close();
        }

        /// <summary>
        /// Alustaa pelialueen ja aloittaa uuden pelin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset(object sender, RoutedEventArgs e)
        {
            // Alusta nappien painausmäärä
            clickCount = 0;

            // Aseta kaikki napit painettavaksi
            TogglePlayButtons(true);

            // Tyhjennä kaikkien nappien teksti
            for (int i = 0; i < 9; i++)
            {
                Button? b = FindName("Button" + (i + 1)) as Button;

                if (b is Button)
                {
                    b.Content = null;
                }
            }

            // Alusta tulostekstin teksti
            TextBlock? resultsText = FindName("Results") as TextBlock;
            if (resultsText is TextBlock)
            {
                resultsText.Text = "Result";
                resultsText.Foreground = Brushes.Black;
            }

            // Alusta viiva
            Line? line = FindName("WLine") as Line;
            if (line is Line)
            {
                line.X1 = 0;
                line.X2 = 0;
                line.Y1 = 0;
                line.Y2 = 0;
            }
        }

        /// <summary>
        /// Tarkistaa jos kukaan on voittanut pelin.
        /// Palauttaa 0 jos ei voittajaa, 1 jos risti on voittanut ja 2 jos nolla on voittanut
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private int CheckForWinner()
        {
            string[] grid = GetButtonGrid();

            // Tarkista rivit
            for (int row = 0; row < 3; row++)
            {
                string checker = grid[0 + (row * 3)];

                for (int col = 0; col < 3; col++)
                {
                    if (!checker.Equals(grid[col + (row * 3)]))
                    {
                        break;
                    }
                    else if (col == 2)
                    {
                        int winner = WinnerToInteger(checker);
                        if (winner == 0)
                        {
                            break;
                        }
                        else
                        {
                            DrawWinnerLine(row * 3, (row * 3) + 2);
                            return winner;
                        }
                    }
                }
            }

            // Tarkista sarakkeet
            for (int col = 0; col < 3; col++)
            {
                string checker = grid[0 + col];

                for (int row = 0; row < 3; row++)
                {
                    if (!checker.Equals(grid[col + (row * 3)]))
                    {
                        break;
                    }
                    else if (row == 2)
                    {
                        int winner = WinnerToInteger(checker);
                        if (winner == 0)
                        {
                            break;
                        }
                        else
                        {
                            DrawWinnerLine(col, col + 6);
                            return winner;
                        }
                    }
                }
            }

            // Tarkista diagonaaliset
            int result = CheckCustom(grid, new int[3] { 0, 4, 8 });
            if (result != 0)
            {
                DrawWinnerLine(0, 8);
                return result;
            }

            result = CheckCustom(grid, new int[3] { 2, 4, 6 });
            if (result != 0)
            {
                DrawWinnerLine(2, 6);
                return result;
            }

            return 0;
        }

        private int WinnerToInteger(string winner)
        {
            if (winner.Equals("N"))
                return 0;
            else if (winner.Equals("X"))
                return 1;
            else if (winner.Equals("O"))
                return 2;

            return 0;
        }

        /// <summary>
        /// Tarkistaa jos kukaan on voittanut annetuissa kohdissa
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="checkedTiles"></param>
        /// <returns></returns>
        private int CheckCustom(string[] grid, int[] checkedTiles)
        {
            for (int i = 0; i < checkedTiles.Length; i++)
            {
                string checker = grid[checkedTiles[0]];

                if (!checker.Equals(grid[checkedTiles[i]]))
                {
                    return 0;
                }
                else if (i == checkedTiles.Length - 1)
                {
                    return WinnerToInteger(checker);
                }
            }

            return 0;
        }

        /// <summary>
        /// Muuttaa kaikkien ristinolla-nappien painamismahdollisuuden
        /// </summary>
        private void TogglePlayButtons(bool toggle)
        {
            for (int i = 0; i < 9; i++)
            {
                Button? b = FindName("Button" + (i + 1)) as Button;

                if (b is Button)
                {
                    b.IsHitTestVisible = toggle;
                }
            }
        }

        private void DrawWinnerLine(int from, int to)
        {
            // Koordinaattiarvot
            int[][] coordinateValues = new int[3][]
            {
                new int[] {83, 79},
                new int[] {245, 241},
                new int[] {415, 400}
            };

            // Grid-koordinaatit
            int[][] gridXY = new int[9][]
            {
                new int[] { coordinateValues[0][0], coordinateValues[0][1] },
                new int[] { coordinateValues[1][0], coordinateValues[0][1] },
                new int[] { coordinateValues[2][0], coordinateValues[0][1] },
                new int[] { coordinateValues[0][0], coordinateValues[1][1] },
                new int[] { coordinateValues[1][0], coordinateValues[1][1] },
                new int[] { coordinateValues[2][0], coordinateValues[1][1] },
                new int[] { coordinateValues[0][0], coordinateValues[2][1] },
                new int[] { coordinateValues[1][0], coordinateValues[2][1] },
                new int[] { coordinateValues[2][0], coordinateValues[2][1] }
            };

            // Aseta viiva
            Line? l = FindName("WLine") as Line;
            if (l is Line)
            {
                l.X1 = gridXY[from][0];
                l.Y1 = gridXY[from][1];
                l.X2 = gridXY[to][0];
                l.Y2 = gridXY[to][1];
            }
        }

        private string[] GetButtonGrid()
        {
            string[] grid = new string[9];
            for (int i = 0; i < 9; i++)
            {
                Button? b = FindName("Button" + (i + 1)) as Button;

                if (b is Button)
                {
                    if (b.Content == null)
                    {
                        grid[i] = "N";
                    }
                    else
                    {
                        grid[i] = b.Content.ToString();
                    }
                }
            }

            return grid;
        }
    }
}
