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

            if (button.Content != null)
            {
                return;
            }

            // Muuta nappi X tai O vuorotellen
            if (clickCount % 2 == 0)
            {
                button.Content = "X";
            }
            else
            {
                button.Content = "O";
            }

            // Lisää nappien painallusmäärää
            clickCount++;
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
