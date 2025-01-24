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
    /// Interaction logic for TableOtkupStavka.xaml
    /// </summary>
    public partial class TableOtkupStavka : Page, ITableOperations
    {

        private string otkupSifra { get; set; }
        public TableOtkupStavka()
        {
            InitializeComponent();
            try
            {
                otkupSifra = "";
                Refresh();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["ErrorOccurred"] as string, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MyDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                // Cast the edited row to the OtkupStavka class
                var editedOtkupStavka = e.Row.Item as OtkupStavka;

                if (editedOtkupStavka != null && editedOtkupStavka.otkup != null && editedOtkupStavka.artikal != null)
                {
                    // If ID is 0, it's a new item
                    MySQLDataAccessFactory.GetOtkupStavkaDataAccess().AddOStavka(editedOtkupStavka);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.Resources["InvalidData"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void deleteRow()
        {
            // Get the selected OtkupStavka
            var selectedOtkupStavka = MyDataGrid.SelectedItem as OtkupStavka;

            if (selectedOtkupStavka != null)
            {
                // Confirm deletion
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmDeletionMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetOtkupStavkaDataAccess().DeleteOStavka(selectedOtkupStavka.otkup.sifra, selectedOtkupStavka.artikal.Sifra);
                        Search(otkupSifra);
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
            var selectedOtkupStavka = MyDataGrid.SelectedItem as OtkupStavka;

            if (selectedOtkupStavka != null)
            {
                // Confirm addition
                var customMessageBox = new CustomMessageBox($"{Application.Current.Resources["ConfirmAddMessage"] as string}", $"{Application.Current.Resources["Yes"] as string}", $"{Application.Current.Resources["No"] as string}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetOtkupStavkaDataAccess().AddOStavka(selectedOtkupStavka);
                        Search(otkupSifra);
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
            MessageBox.Show(Application.Current.Resources["UnsupportedUpdate"] as string, "", MessageBoxButton.OK, MessageBoxImage.Information);

            /*var selectedOtkupStavka = MyDataGrid.SelectedItem as OtkupStavka;

            if (selectedOtkupStavka != null && selectedOtkupStavka.otkup != null && selectedOtkupStavka.artikal != null)
            {
                // Confirm update
                var customMessageBox = new CustomMessageBox($"{Resource.ConfirmUpdateMessage}", $"{Resource.Yes}", $"{Resource.No}");

                // Show the message box and check the result
                bool? result = customMessageBox.ShowDialog();

                if (result == true)
                {
                    try
                    {
                        MySQLDataAccessFactory.GetOtkupStavkaDataAccess().UpdateOStavka(selectedOtkupStavka);
                        Search(otkupSifra);
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Database", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }*/
        }

        public void Refresh()
        {
            var data = new List<OtkupStavka>();
            this.DataContext = data;
            MyDataGrid.ItemsSource = data;

            var artikli = MySQLDataAccessFactory.GetArtikalDataAccess().GetArtikli();
            var comboBoxColumnArtikal = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Artikal");

            if (comboBoxColumnArtikal != null)
            {
                comboBoxColumnArtikal.ItemsSource = artikli; // Assign the collection to the ItemsSource
            }
            var otkupi = MySQLDataAccessFactory.GetOtkupDataAccess().GetOtkupi();
            var comboBoxColumnOtkup = MyDataGrid.Columns
                .OfType<DataGridComboBoxColumn>()
                .FirstOrDefault(c => c.Header.ToString() == "Otkup");

            if (comboBoxColumnOtkup != null)
            {
                comboBoxColumnOtkup.ItemsSource = otkupi; // Assign the collection to the ItemsSource
            }

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
                // Get the OtkupStavka by Otkup Šifra
                var foundOtkupStavka = MySQLDataAccessFactory.GetOtkupStavkaDataAccess().SearchOtkupStavka(text);

                if (foundOtkupStavka != null)
                {
                    // Display in the DataGrid
                    MyDataGrid.ItemsSource = foundOtkupStavka;
                    otkupSifra = text;
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


