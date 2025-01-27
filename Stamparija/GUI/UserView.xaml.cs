using Google.Protobuf.Compiler;
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
using Stamparija.DAO;
using Stamparija.DAO.connection;
using static MaterialDesignThemes.Wpf.Theme;
using System.Collections.Specialized;
using System.Drawing;
using Stamparija.DTO;

namespace Stamparija.GUI
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Page
    {
        public ITableOperations currentPage { get; set; }

        public static TextBlock infoTextBlock { get; set; }
        public UserView()
        {
            InitializeComponent();
            InitializeAdminControls();
        }
        private void InitializeAdminControls()
        {
            try
            {
                if (MainWindow.userManagement.isAdmin()) // Replace with your admin check
                {
                    zaposleniButton.Visibility = Visibility.Visible;
                    // Add to your StackPanel
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ShowProfileSettings(object sender, RoutedEventArgs e)
        {
            currentPage = null;
            WorkFrame.Navigate(new ProfileSettings());
            stackPanelColor(sender, e);
        }
        private void ShowZaposleni(object sender, RoutedEventArgs e)
        {
            currentPage = new TableZaposleni();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }
        private void ShowSaradnici(object sender, RoutedEventArgs e)
        {
            currentPage = new TableSaradnik();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }
        private void ShowTelefoni(object sender, RoutedEventArgs e)
        {
            currentPage = new TableTelefon();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }
        private void ShowZiroracuni(object sender, RoutedEventArgs e)
        {
            currentPage = new TableZiroracun();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }

        private void ShowArtikli(object sender, RoutedEventArgs e)
        {
            currentPage = new TableArtikal();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }

        private void ShowMjesta(object sender, RoutedEventArgs e)
        {
            currentPage = new TableMjesto();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }

        private void ShowOtkupi(object sender, RoutedEventArgs e)
        {
            currentPage = new TableOtkup();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }
        private void ShowFakture(object sender, RoutedEventArgs e)
        {
            currentPage = new TableFaktura();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }

        private void ShowOtkupStavke(object sender, RoutedEventArgs e)
        {
            currentPage = new TableOtkupStavka();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
            SearchBox.Focus();
        }
        private void ShowProizvodjaci(object sender, RoutedEventArgs e)
        {
            currentPage = new TableProizvodjac();
            WorkFrame.Navigate(currentPage);
            stackPanelColor(sender, e);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SearchBox.Text) && currentPage!=null)
                {
                    currentPage.Refresh();
                }
                else if (currentPage!=null) SearchButtonClick(sender, e);
            }catch (Exception ex)
            {

            }
            
        }

        private void stackPanelColor(object sender, RoutedEventArgs e)
        {
            try
            {
                // Loop through all buttons in the StackPanel
                foreach (var child in MyStackPanel.Children)
                {
                    if (child is System.Windows.Controls.Button button)
                    {
                        // Reset all buttons' background
                        button.Background = System.Windows.Media.Brushes.Transparent;
                    }
                }

                // Change the clicked button's background to white
                if (sender is System.Windows.Controls.Button clickedButton &&
                    MyStackPanel.Children.Contains(clickedButton))
                {
                    clickedButton.Background = (SolidColorBrush)FindResource("Background");
                }
            }catch (Exception ex) {
            }
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentPage!= null)
            {
                try
                {
                    currentPage.deleteRow();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentPage != null)
            {
                try
                {
                    currentPage.addRow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentPage != null)
            {
                try
                {
                    currentPage.updateRow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentPage != null)
            {
                string text = SearchBox.Text;
                try
                {   if (!string.IsNullOrEmpty(text))
                    currentPage.Search(text);
                    else
                        MessageBox.Show(Application.Current.Resources["EmptySearch"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentPage != null)
            {
                SearchBox.Text="";
                try
                {
                        currentPage.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
