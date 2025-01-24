using Stamparija.theme;
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
using static MaterialDesignThemes.Wpf.Theme;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace Stamparija.GUI
{
    /// <summary>
    /// Interaction logic for ProfileSettings.xaml
    /// </summary>
    public partial class ProfileSettings : Page
    {
        public ProfileSettings()
        {
            InitializeComponent();
            ThemeComboBox.Tag = Settings.Default.Theme=="light"? "Light" :"Dark";
            FontSizeComboBox.Tag = Settings.Default.FontSize == "small" ? "1" : 
                                   Settings.Default.FontSize == "medium"? "2" : "3";
            LanguageComboBox.Tag = Settings.Default.Theme == "srp" ? "Srpski" : "English";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ThemeComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    // Get the Tag of the selected ComboBoxItem
                    var selectedTheme = selectedItem.Tag.ToString();

                    // Apply theme
                    if (selectedTheme == "Dark")
                    {
                        UserManagement.userSettings.Theme = "dark";
                    }
                    else
                    {
                        UserManagement.userSettings.Theme = "light";
                    } }

                UserManagement.SaveUserSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["ErrorOccurred"] as string,"", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }


        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    // Get the Tag of the selected ComboBoxItem
                    var selectedTheme = selectedItem.Tag.ToString();
                    // Apply theme
                    if (selectedTheme == "Srpski")
                    {
                        UserManagement.userSettings.Language = "srp";

                    }
                    else
                    {
                        UserManagement.userSettings.Language = "eng";
                    }
                    UserManagement.SaveUserSettings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["ErrorOccurred"] as string, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmLogoutMessage"]}", $"{Application.Current.Resources["Yes"]}", $"{Application.Current.Resources["No"]}");

            // Show the message box and check the result
            bool? result = customMessageBox.ShowDialog();

            if (result == true)
            {
                try
                {
                    MainWindow.MAIN_FRAME.Navigate(new Login());
                }catch(Exception ex) { }
            }
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FontSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    // Get the Tag of the selected ComboBoxItem
                    var selectedFontSize = selectedItem.Tag.ToString();
                    // Apply font size
                    if (selectedFontSize == "1")
                    {
                        UserManagement.userSettings.FontSize = "small";
                    }
                    else if (selectedFontSize == "2")
                    {
                        UserManagement.userSettings.FontSize = "medium";
                    }
                    else if (selectedFontSize == "3")
                    {
                        UserManagement.userSettings.FontSize = "large";
                    }
                }
                UserManagement.SaveUserSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["ErrorOccurred"] as string, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
