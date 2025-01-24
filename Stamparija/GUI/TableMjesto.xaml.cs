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
    /// Interaction logic for TableMjesto.xaml
    /// </summary>
    public partial class TableMjesto : Page, ITableOperations
    {
        public TableMjesto()
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
            try
            {
                // Cast the edited row to the Mjesto class
                var editedMjesto = e.Row.Item as Mjesto;

                if (editedMjesto != null && !string.IsNullOrEmpty(editedMjesto.PostanskiBroj))
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetMjestoDataAccess().AddMjesto(editedMjesto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void deleteRow()
        {
            // Get the selected Mjesto
            var selectedMjesto = MyDataGrid.SelectedItem as Mjesto;

            if (selectedMjesto != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetMjestoDataAccess().DeleteMjesto(selectedMjesto.PostanskiBroj);
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
            var selectedMjesto = MyDataGrid.SelectedItem as Mjesto;

            if (selectedMjesto != null)
            {
                if (string.IsNullOrEmpty(selectedMjesto.PostanskiBroj) ||
                    string.IsNullOrEmpty(selectedMjesto.Naziv))
                {
                    MessageBox.Show("Please fill all fields.", "Invalid data", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            MySQLDataAccessFactory.GetMjestoDataAccess().AddMjesto(selectedMjesto);
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
            var selectedMjesto = MyDataGrid.SelectedItem as Mjesto;

            if (selectedMjesto != null && !string.IsNullOrEmpty(selectedMjesto.PostanskiBroj))
            {
                if (string.IsNullOrEmpty(selectedMjesto.PostanskiBroj) ||
                    string.IsNullOrEmpty(selectedMjesto.Naziv)) {
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
                            MySQLDataAccessFactory.GetMjestoDataAccess().UpdateMjesto(selectedMjesto);
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
            var data = MySQLDataAccessFactory.GetMjestoDataAccess().GetMjesta("");
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
        }

        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show(Application.Current.Resources["EmptySearch"] as string, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Get the Mjesto by Postanski Broj
                var foundMjesto = MySQLDataAccessFactory.GetMjestoDataAccess().GetMjesta(text);

                if (foundMjesto != null && foundMjesto.Count > 0)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundMjesto;
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
