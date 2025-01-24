using MySql.Data.MySqlClient;
using Stamparija.DAO.connection;
using Stamparija.DTO;
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

namespace Stamparija.GUI
{
    /// <summary>
    /// Interaction logic for TableArtikal.xaml
    /// </summary>
    public partial class TableArtikal : Page, ITableOperations
    {
        public TableArtikal()
        {
            InitializeComponent();
            try
            {
                Refresh();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MyDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
                // Cast the edited row to the Artikal class
               /* var selectedArtikal = e.Row.Item as Artikal;

            if (selectedArtikal != null && !string.IsNullOrEmpty(selectedArtikal.Sifra))
            {

                var customMessageBox = new CustomMessageBox($"{Resource.ConfirmUpdateMessage}", $"{Resource.Yes}", $"{Resource.No}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {*/
                    try
                    {
                        updateRow();
                        /*MySQLDataAccessFactory.GetArtikalDataAccess().UpdateArtikal(selectedArtikal);
                        Refresh();*/
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
               /* }
            }*/
            }

        public void deleteRow()
        {
            // Get the selected Artikal
            var selectedArtikal = MyDataGrid.SelectedItem as Artikal;

            if (selectedArtikal != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetArtikalDataAccess().DeleteArtikal(selectedArtikal.Sifra);
                        Refresh();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["NoSelectionMessage"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void addRow()
        {
            var selectedArtikal = MyDataGrid.SelectedItem as Artikal;

            if (selectedArtikal != null)
            {
                if(string.IsNullOrEmpty(selectedArtikal.Sifra) ||
                   string.IsNullOrEmpty(selectedArtikal.Naziv) || 
                   string.IsNullOrEmpty(selectedArtikal.Kategorija) || 
                   selectedArtikal.Proizvodjac == null)
                {
                    MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Confirm addition
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmAddMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetArtikalDataAccess().AddArtikal(selectedArtikal);
                        Refresh();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["NoSelectionMessage"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void updateRow()
        {
            var selectedArtikal = MyDataGrid.SelectedItem as Artikal;

            if (selectedArtikal != null)
            {
                if (string.IsNullOrEmpty(selectedArtikal.Sifra) ||
                   string.IsNullOrEmpty(selectedArtikal.Naziv) ||
                   string.IsNullOrEmpty(selectedArtikal.Kategorija) ||
                   selectedArtikal.Proizvodjac == null)
                {
                  
                    MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Confirm update
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmUpdateMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetArtikalDataAccess().UpdateArtikal(selectedArtikal);
                        Refresh();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["NoSelectionMessage"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Refresh()
        {
            var data = MySQLDataAccessFactory.GetArtikalDataAccess().GetArtikli();
            var proizvodjaci = MySQLDataAccessFactory.GetProizvodjacDataAccess().GetProizvodjaci();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
            var comboBoxColumn = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Proizvođač");

            if (comboBoxColumn != null)
            {
                comboBoxColumn.ItemsSource = proizvodjaci; // Assign the collection to the ItemsSource
            }
        }

        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(Application.Current.Resources["EmptySearch"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                // Get the Artikal by Sifra
                var foundArtikal = MySQLDataAccessFactory.GetArtikalDataAccess().SearchArtikli(text);

                if (foundArtikal != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundArtikal;
                }
                else
                {
                    MessageBox.Show(Application.Current.Resources["NoDataInfo"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}