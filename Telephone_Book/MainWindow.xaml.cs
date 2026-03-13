using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Telephone_Book
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Connection_sql db = new Connection_sql();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshDataGrid()
        {
            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM PHONE", con);
                int count = (int)cmdCount.ExecuteScalar();

                Model_Data[] temp = new Model_Data[count];

                string query = "SELECT PhoneId, NameCust, NumberCust FROM PHONE";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    temp[i++] = new Model_Data()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2)
                    };
                }
                myDataGrid.ItemsSource = temp;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = db.GetConnection())
            {
                string query = "INSERT INTO PHONE (NameCust, NumberCust) VALUES (@Name, @Number)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Number", txtPhone.Text);
                cmd.ExecuteNonQuery();
            }
            RefreshDataGrid();
            MessageBox.Show("Add Is Done");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem is Model_Data selected)
            {
                using (SqlConnection con = db.GetConnection())
                {
                    string query = "UPDATE PHONE SET NameCust=@Name, NumberCust=@Numbere WHERE PhoneId=@Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", selected.Id);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Numbere", txtPhone.Text);
                    cmd.ExecuteNonQuery();
                }

                RefreshDataGrid();
            }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a contact to delete.");
                return;
            }
            Model_Data selected = (Model_Data)myDataGrid.SelectedItem;

            try
            {
                using (SqlConnection con = db.GetConnection())
                {
                    string query = "DELETE FROM PHONE WHERE PhoneId=@Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", selected.Id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Contact deleted successfully.");
                    else
                        MessageBox.Show("No contact was deleted.");
                }

                Model_Data[] currentList = (Model_Data[])myDataGrid.ItemsSource;

                if (currentList != null)
                {
                    Model_Data[] newArray = new Model_Data[currentList.Length - 1];

                    int j = 0;
                    for (int i = 0; i < currentList.Length; i++)
                    {
                        if (currentList[i].Id != selected.Id)
                        {
                            newArray[j++] = currentList[i];
                        }
                    }
                    myDataGrid.ItemsSource = newArray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting contact: " + ex.Message);
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            string searchText = txtName.Text;

            using (SqlConnection con = db.GetConnection())
            {
                SqlCommand cmdCount = new SqlCommand(
                    "SELECT COUNT(*) FROM PHONE WHERE NameCust LIKE @Name", con);
                cmdCount.Parameters.AddWithValue("@Name", "%" + searchText + "%");
                int count = (int)cmdCount.ExecuteScalar();


                Model_Data[] temp = new Model_Data[count];

                string query = "SELECT PhoneId, NameCust, NumberCust FROM PHONE WHERE NameCust LIKE @Name";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", "%" + searchText + "%");
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    temp[i++] = new Model_Data()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2)
                    };
                }
                myDataGrid.ItemsSource = temp;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

