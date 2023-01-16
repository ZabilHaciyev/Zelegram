using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zelegram.Views
{
    /// <summary>
    /// Interaction logic for KeyWindow.xaml
    /// </summary>
    public partial class KeyWindow : Window
    {
        private string _key;
        public KeyWindow(string key)
        {
            _key = key;
            InitializeComponent();
        }
        public bool IsClosed { get; private set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_key == KeyTextBox.Text.ToString())
            {
                Error.Visibility = Visibility.Hidden;
                MessageBox.Show("Congrats, Welcome ;)");

                this.Close();
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }
        }
    }
}
