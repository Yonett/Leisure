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
using System.Windows.Shapes;

namespace Leisure.Views
{
    /// <summary>
    /// Логика взаимодействия для LeisureMessageBox.xaml
    /// </summary>
    public partial class LeisureMessageBox : Window
    {
        public LeisureMessageBox(string message, string buttonText)
        {
            InitializeComponent();
            MessageBlock.Text = message;
            ActionButton.Content = buttonText;
            this.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
