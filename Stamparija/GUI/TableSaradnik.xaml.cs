using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;
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
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class TableSaradnik : Page, ITableOperations
    {

        //public Saradnik SelectedSaradnik { get; set; }
        public TableSaradnik()
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
        
        //popraviti
        private void MyDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                // Cast the edited row to the Saradnik class
                var editedSaradnik = e.Row.Item as Saradnik;

                if (editedSaradnik != null && editedSaradnik.Sifra != "")
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetSaradnikDataAccess().AddSaradnik(editedSaradnik);
                }
            }
            catch(Exception ex)
            { 
                MessageBox.Show(ex.Message, "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void deleteRow()
        {
            // Get the selected Saradnik
            var selectedSaradnik = MyDataGrid.SelectedItem as Saradnik;

            if (selectedSaradnik != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetSaradnikDataAccess().DeleteSaradnik(selectedSaradnik.Sifra);
                        Refresh();
                    } catch (MySqlException ex)
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
            var selectedSaradnik = MyDataGrid.SelectedItem as Saradnik;
            var selectedMjesto = MyDataGrid.SelectedItem as Mjesto;

            if (selectedSaradnik != null) {
                if (string.IsNullOrEmpty(selectedSaradnik.Sifra) ||
                    string.IsNullOrEmpty(selectedSaradnik.Vrsta) ||
                    selectedSaradnik.Mjesto == null)
                {
                    MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Confirm deletion
                //selectedSaradnik.Mjesto = selectedMjesto;
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmAddMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetSaradnikDataAccess().AddSaradnik(selectedSaradnik);
                        Refresh();
                    } catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(Application.Current.Resources["NoSelection"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void updateRow()
        {
            var selectedSaradnik = MyDataGrid.SelectedItem as Saradnik;
            var selectedMjesto = MyDataGrid.SelectedItem as Mjesto;

            if (selectedSaradnik != null)
            {
                if (string.IsNullOrEmpty(selectedSaradnik.Sifra) || 
                    string.IsNullOrEmpty(selectedSaradnik.Vrsta) ||
                    selectedSaradnik.Mjesto == null)
                {
                    MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                // Confirm deletion
                //selectedSaradnik.Mjesto = selectedMjesto;
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmUpdateMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetSaradnikDataAccess().UpdateSaradnik(selectedSaradnik);
                        Refresh();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            { //todo message
                MessageBox.Show(Application.Current.Resources["NoSelectionMessage"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Refresh()
        {
            var data = MySQLDataAccessFactory.GetSaradnikDataAccess().GetSaradnici();
            var mjesta = MySQLDataAccessFactory.GetMjestoDataAccess().GetMjesta("");
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
            var comboBoxColumn = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Poštanski broj");

            if (comboBoxColumn != null)
            {
                comboBoxColumn.ItemsSource = mjesta; // Assign the collection to the ItemsSource
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
                // Get the Saradnik by Sifra
                var foundSaradnik = MySQLDataAccessFactory.GetSaradnikDataAccess().SearchSaradnik(text);

                if (foundSaradnik != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundSaradnik;
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
