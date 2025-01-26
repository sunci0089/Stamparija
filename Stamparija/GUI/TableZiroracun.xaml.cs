using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for TableZiroracun.xaml
    /// </summary>
    public partial class TableZiroracun : Page, ITableOperations
    {
        public TableZiroracun()
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

        private void MyDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (MyDataGrid.SelectedItem != null)
            {
                MyDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            }
        }

        public void deleteRow()
        {
            // Get the selected Ziroracun
            var selectedZiroracun = MyDataGrid.SelectedItem as Ziroracun;

            if (selectedZiroracun != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetZiroracunDataAccess().DeleteZiroracun(selectedZiroracun.brojRacuna);
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
            var selectedZiroracun = MyDataGrid.SelectedItem as Ziroracun;

            if (selectedZiroracun != null)
            {
                if (string.IsNullOrEmpty(selectedZiroracun.brojRacuna) || selectedZiroracun.saradnik == null || string.IsNullOrEmpty(selectedZiroracun.saradnik.Sifra) || string.IsNullOrEmpty(selectedZiroracun.banka))
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
                        MySQLDataAccessFactory.GetZiroracunDataAccess().AddZiroracun(selectedZiroracun);
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
            var selectedZiroracun = MyDataGrid.SelectedItem as Ziroracun;

            if (selectedZiroracun != null)
            {
                if (string.IsNullOrEmpty(selectedZiroracun.brojRacuna) || selectedZiroracun.saradnik == null || string.IsNullOrEmpty(selectedZiroracun.saradnik.Sifra) || string.IsNullOrEmpty(selectedZiroracun.banka))
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
                        MySQLDataAccessFactory.GetZiroracunDataAccess().UpdateZiroracun(selectedZiroracun);
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
            var data = MySQLDataAccessFactory.GetZiroracunDataAccess().GetZiroracun();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
            var saradnici = MySQLDataAccessFactory.GetSaradnikDataAccess().GetSaradnici();
            var comboBoxColumn = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Saradnik šifra");

            if (comboBoxColumn != null)
            {
                comboBoxColumn.ItemsSource = saradnici; // Assign the collection to the ItemsSource
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
                // Get the Ziroracun by brojRacuna
                var foundZiroracun = MySQLDataAccessFactory.GetZiroracunDataAccess().SearchZiroracun(text);

                if (foundZiroracun != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundZiroracun;
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


