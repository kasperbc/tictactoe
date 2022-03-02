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

            // Lisää nappien painallusmäärää
            clickCount++;

            // Tarkistaa, onko peli päättynyt
            // Ota jokainen nappi
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

            // Tarkista jos kukaan on voittanut
            int winner = CheckForWinner(grid);

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
                        resultsText.Text = "Stalemate";
                        resultsText.Foreground = Brushes.DarkGray;
                    }

                    // Ottaa kaikki ristinolla-napit pois käytöstä
                    TogglePlayButtons(false);
                }
            }
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
        }

        /// <summary>
        /// Tarkistaa jos kukaan on voittanut pelin.
        /// Palauttaa 0 jos ei voittajaa, 1 jos risti on voittanut ja 2 jos nolla on voittanut
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        private int CheckForWinner(string[] grid)
        {
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
                            return winner;
                        }
                    }
                }
            }

            // Tarkista diagonaaliset
            int result = CheckCustom(grid, new int[3] { 0, 4, 8 });
            if (result != 0)
            {
                return result;
            }

            result = CheckCustom(grid, new int[3] { 2, 4, 6 });
            if (result != 0)
            {
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
    }
}
