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
    /// Interaction logic for TableFaktura.xaml
    /// </summary>
    public partial class TableFaktura : Page, ITableOperations
    {
        public TableFaktura()
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
                // Cast the edited row to the Faktura class
                var editedFaktura = e.Row.Item as Faktura;

                if (editedFaktura != null && editedFaktura.Sifra != "")
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetFakturaDataAccess().AddFaktura(editedFaktura);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void deleteRow()
        {// Get the selected Faktura
            var selectedFaktura = MyDataGrid.SelectedItem as Faktura;

            if (selectedFaktura != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetFakturaDataAccess().DeleteFaktura(selectedFaktura.Sifra);
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
            var selectedFaktura = MyDataGrid.SelectedItem as Faktura;

            if (selectedFaktura != null)
            {
                if (string.IsNullOrEmpty(selectedFaktura.Sifra) ||
                    string.IsNullOrEmpty(selectedFaktura.NacinPlacanja) ||
                    string.IsNullOrEmpty(selectedFaktura.VrstaUplate) ||
                    string.IsNullOrEmpty(selectedFaktura.DatumVrijeme.ToString())
                    )
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
                        MySQLDataAccessFactory.GetFakturaDataAccess().AddFaktura(selectedFaktura);
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
            var selectedFaktura = MyDataGrid.SelectedItem as Faktura;

            if (selectedFaktura != null)
            {
                if (string.IsNullOrEmpty(selectedFaktura.Sifra) ||
                    string.IsNullOrEmpty(selectedFaktura.NacinPlacanja) ||
                    string.IsNullOrEmpty(selectedFaktura.VrstaUplate) ||
                    string.IsNullOrEmpty(selectedFaktura.DatumVrijeme.ToString())
                    )
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
                        MySQLDataAccessFactory.GetFakturaDataAccess().UpdateFaktura(selectedFaktura);
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
            var data = MySQLDataAccessFactory.GetFakturaDataAccess().GetFakture();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;
            var kupoprodaja = new List<string> { "KUPOVINA", "PRODAJA" };
            var comboBoxColumn = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Vrsta uplate");

            if (comboBoxColumn != null)
            {
                comboBoxColumn.ItemsSource = kupoprodaja; // Assign the collection to the ItemsSource
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
                // Get the Faktura by Sifra
                var foundFaktura = MySQLDataAccessFactory.GetFakturaDataAccess().SearchFakture(text);

                if (foundFaktura != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundFaktura;
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