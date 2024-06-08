using Microsoft.Win32;
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
using System.Data.SQLite;
using treelilnick.connector;
using System.Collections.Generic;
using treelilnick.regex;

namespace treelilnick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            bool? isKMP = algoTypeToggle.IsChecked;
            //MessageBox.Show("status: " + isKMP.ToString());
            //string currentDirectory = Environment.CurrentDirectory;

            // Show the current directory in a message box
            //MessageBox.Show("Current Directory: " + currentDirectory
            if (isKMP.HasValue && (bool)isKMP)
            {
                timeLabel.Text = "Checked";
            }
            else
            {
                timeLabel.Text = "Unchecked";
            }
            Database conn = new Database();
            //List<Pair<string, string>> pairs = conn.GetNamaAndNIK();
            //foreach (Pair<string,string> pair in pairs)
            //{
            //    listView.Items.Add(pair.First + " : " + pair.Second);
            //}
            //List<Pair<string, string>> listAlay = conn.GetNamaAndNIK();
            //Pair<string, string> matched;
            List<Pair<string, string>> pragma = conn.GetDataSidikJari();
            foreach(Pair<string, string> pair in pragma)
            {
                listView.Items.Add(pair.Second + " : " + pair.First);
            }
            //try
            //{
            //    matched = AlayRegex.SearchAlay("Ab Roblin", listAlay);

            //    List<string> data = conn.GetBioData(matched.Second);
            //    foreach (string s in data)
            //    {
            //        listView.Items.Add(s);
            //    }
            //}
            //catch (Exception err)
            //{
            //    Console.WriteLine(err.Message);
            //}
        }

        private void ButtonUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files | *.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                inputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            listView.Items.Clear();
        }

        private void ConectionSQLite()
        {
            //string databasePath = "mydatabase.db";
            //string connectionString = "Data Source=citra.db;Version=3;";

            //using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            //{
            //    connection.Open();

            //    // Create a new table if it doesn't exist
            //    string createTableQuery = "SELECT * FROM sidik_jari";

            //    using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, connection))
            //    {
            //        using (SQLiteDataReader reader = createTableCommand.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                string berkas = reader["berkas_citra"].ToString();
            //                string name = reader["nama"].ToString();
            //                MessageBox.Show(berkas +" : "+ name);
            //            }
            //        }
            //    }

            //    Console.WriteLine("Table created successfully (or already exists).");

            //    connection.Close();
            //}
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files | *.bmp;*.jpg;*.png";
            //openFileDialog.FilterIndex = 1;
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    outputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            //}
            //MessageBox.Show(new Database().getName());
        }
    }
}