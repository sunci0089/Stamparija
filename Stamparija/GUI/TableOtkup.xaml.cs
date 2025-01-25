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
    /// Interaction logic for TableOtkup.xaml
    /// </summary>
    public partial class TableOtkup : Page, ITableOperations
    {
        public TableOtkup()
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
                // Cast the edited row to the Otkup class
                var editedOtkup = e.Row.Item as Otkup;

                if (editedOtkup != null && !string.IsNullOrEmpty(editedOtkup.sifra))
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetOtkupDataAccess().AddOtkup(editedOtkup);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void deleteRow()
        {
            // Get the selected Otkup
            var selectedOtkup = MyDataGrid.SelectedItem as Otkup;

            if (selectedOtkup != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletion"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetOtkupDataAccess().DeleteOtkup(selectedOtkup.sifra);
                        MySQLDataAccessFactory.GetFakturaDataAccess().DeleteFaktura(selectedOtkup.faktura.Sifra);
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
            var selectedOtkup = MyDataGrid.SelectedItem as Otkup;

            if (selectedOtkup != null)
            {
                // Confirm addition
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmAddMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetFakturaDataAccess().AddFaktura(selectedOtkup.faktura);
                        MySQLDataAccessFactory.GetOtkupDataAccess().AddOtkup(selectedOtkup);
                        MessageBox.Show(Application.Current.Resources["RedemptionAddedMessage"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var selectedOtkup = MyDataGrid.SelectedItem as Otkup;

            if (selectedOtkup != null && !string.IsNullOrEmpty(selectedOtkup.sifra))
            {
                // Confirm update
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmUpdateMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetFakturaDataAccess().UpdateFaktura(selectedOtkup.faktura);
                        MySQLDataAccessFactory.GetOtkupDataAccess().UpdateOtkup(selectedOtkup);
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
            var data = MySQLDataAccessFactory.GetOtkupDataAccess().GetOtkupi();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;

            var saradnici = MySQLDataAccessFactory.GetSaradnikDataAccess().GetSaradnici();
            var comboBoxColumn = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Saradnik");

            if (comboBoxColumn != null)
            {
                comboBoxColumn.ItemsSource = saradnici; // Assign the collection to the ItemsSource
            }
            var kupoprodaja = new List<string> { "KUPOVINA", "PRODAJA" };
            var comboBoxColumn2 = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Vrsta uplate");

            if (comboBoxColumn2 != null)
            {
                comboBoxColumn2.ItemsSource = kupoprodaja; // Assign the collection to the ItemsSource
            }
            var ziroracuni = MySQLDataAccessFactory.GetZiroracunDataAccess().GetZiroracun();
            ziroracuni.Insert(0, new Ziroracun("",null,""));
            var comboBoxColumnArtikal = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Žiroračun saradnika");

            if (comboBoxColumnArtikal != null)
            {
                comboBoxColumnArtikal.ItemsSource = ziroracuni; // Assign the collection to the ItemsSource
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
                // Get the Otkup by Sifra
                var foundOtkup = MySQLDataAccessFactory.GetOtkupDataAccess().SearchOtkup(text);

                if (foundOtkup != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundOtkup;
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


