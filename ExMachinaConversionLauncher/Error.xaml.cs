using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExMachinaConversionLauncher
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        private readonly bool _closeApp;
        public Error(string validationError, bool closeApp = true)
        {
            _closeApp = closeApp;
            InitializeComponent();
            ErroeTextBlock.Text = validationError;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_closeApp)
            {
                Application.Current.Shutdown();
                this.Close();
            }
            else this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }


    }
}
