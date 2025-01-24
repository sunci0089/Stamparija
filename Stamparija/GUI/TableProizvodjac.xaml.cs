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
    /// Interaction logic for TableProizvodjac.xaml
    /// </summary>
    public partial class TableProizvodjac : Page, ITableOperations
    {
        public TableProizvodjac()
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
                MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                // Cast the edited row to the Proizvodjac class
                var editedProizvodjac = e.Row.Item as Proizvodjac;

                if (editedProizvodjac != null && !string.IsNullOrEmpty(editedProizvodjac.Sifra))
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetProizvodjacDataAccess().AddProizvodjac(editedProizvodjac);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void deleteRow()
        {
            // Get the selected Proizvodjac
            var selectedProizvodjac = MyDataGrid.SelectedItem as Proizvodjac;

            if (selectedProizvodjac != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetProizvodjacDataAccess().DeleteProizvodjac(selectedProizvodjac.Sifra);
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
            var selectedProizvodjac = MyDataGrid.SelectedItem as Proizvodjac;

            if (selectedProizvodjac != null)
            {
                if (string.IsNullOrEmpty(selectedProizvodjac.Sifra) ||
                    string.IsNullOrEmpty(selectedProizvodjac.Ime) ||
                    string.IsNullOrEmpty(selectedProizvodjac.DrzavaPorijekla))
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
                        MySQLDataAccessFactory.GetProizvodjacDataAccess().AddProizvodjac(selectedProizvodjac);
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
                MessageBox.Show(Application.Current.Resources["NoSelectionMessage"] as string,"", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void updateRow()
        {
            var selectedProizvodjac = MyDataGrid.SelectedItem as Proizvodjac;

            if (selectedProizvodjac != null)
            {
                if (string.IsNullOrEmpty(selectedProizvodjac.Sifra) ||
                    string.IsNullOrEmpty(selectedProizvodjac.Ime) ||
                    string.IsNullOrEmpty(selectedProizvodjac.DrzavaPorijekla))
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
                            MySQLDataAccessFactory.GetProizvodjacDataAccess().UpdateProizvodjac(selectedProizvodjac);
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
            var data = MySQLDataAccessFactory.GetProizvodjacDataAccess().GetProizvodjaci();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
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
                // Get the Proizvodjac by Sifra
                var foundProizvodjac = MySQLDataAccessFactory.GetProizvodjacDataAccess().SearchProizvodjac(text);

                if (foundProizvodjac != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundProizvodjac;
                }
                else
                {
                    MessageBox.Show(Application.Current.Resources["NoDataInfo"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
