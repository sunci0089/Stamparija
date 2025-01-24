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
using static MaterialDesignThemes.Wpf.Theme;

namespace Stamparija.GUI
{
    /// <summary>
    /// Interaction logic for TableZaposleni.xaml
    /// </summary>
    public partial class TableZaposleni : Page, ITableOperations
    {
        public TableZaposleni()
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
                // Cast the edited row to the Zaposleni class
                var editedZaposleni = e.Row.Item as Zaposleni;

                if (editedZaposleni != null && editedZaposleni.id != 0)
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetZaposleniDataAccess().AddZaposleni(editedZaposleni);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void deleteRow()
        {
            // Get the selected Zaposleni
            var selectedZaposleni = MyDataGrid.SelectedItem as Zaposleni;

            if (selectedZaposleni != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetZaposleniDataAccess().DeleteZaposleni(selectedZaposleni.id);
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
            var selectedZaposleni = MyDataGrid.SelectedItem as Zaposleni;

            if (selectedZaposleni != null)
            {
                // Confirm addition
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmAddMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetZaposleniDataAccess().AddZaposleni(selectedZaposleni);
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
            var selectedZaposleni = MyDataGrid.SelectedItem as Zaposleni;

            if (selectedZaposleni != null && selectedZaposleni.id != 0)
            {
                // Confirm update
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmUpdateMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetZaposleniDataAccess().UpdateZaposleni(selectedZaposleni);
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
            var data = MySQLDataAccessFactory.GetZaposleniDataAccess().GetZaposleni();
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
                // Get the Zaposleni by ID
                var foundZaposleni = MySQLDataAccessFactory.GetZaposleniDataAccess().SearchZaposleni(text);

                if (foundZaposleni != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundZaposleni;
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

