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
using static MaterialDesignThemes.Wpf.Theme;

namespace Stamparija.GUI
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {   
            public bool IsButton1Clicked { get; private set; }
            public bool IsButton2Clicked { get; private set; }

            public CustomMessageBox(string message, string button1Text, string button2Text)
            {
                InitializeComponent();
                MessageText.Text = message;
                Button1.Content = button1Text;
                Button2.Content = button2Text;
            }

            private void Button1_Click(object sender, RoutedEventArgs e)
            {
                IsButton1Clicked = true;
                this.DialogResult = true;
                this.Close();
            }

            private void Button2_Click(object sender, RoutedEventArgs e)
            {
                IsButton2Clicked = true;
                this.DialogResult = false;
                this.Close();
            }
        }
    }



