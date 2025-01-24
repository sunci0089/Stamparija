using Stamparija.GUI;
using Stamparija.theme;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stamparija
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static UserManagement userManagement = new UserManagement();
        public static Frame MAIN_FRAME;
        public MainWindow()
        {
            try
            {
                AppTheme.LoadDefaultTheme();
                InitializeComponent();
                ShowLoginPage();
                MAIN_FRAME = MainFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowLoginPage()
        {
            MainFrame.Navigate(new Login());
        }

        public void ShowUserView()
        {
            MainFrame.Navigate(new UserView());
        }

        private void LightThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("theme/LightTheme.xaml", UriKind.Relative));
        }

        private void DarkThemeClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeTheme(new Uri("theme/DarkTheme.xaml", UriKind.Relative));
        }

        private void SmallFontClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeFontSize(12,14,16);
        }

        private void MediumFontClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeFontSize(14, 16, 18);
        }

        private void LargeFontClick(object sender, RoutedEventArgs e)
        {
            AppTheme.ChangeFontSize(16, 18, 20);
        }
        private void EnglishClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.Language = "en";
            Settings.Default.Save();
            AppTheme.LoadLanguage();
        }
        private void SerbianClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.Language = "srp";
            Settings.Default.Save();
            AppTheme.LoadLanguage();
        }

    }
}