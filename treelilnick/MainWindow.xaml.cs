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
using System.Diagnostics;
using System.Windows.Shapes;
using System.Data.SQLite;
using treelilnick.connector;
using treelilnick.imageloader;
using treelilnick.preproccess;
using treelilnick.algorithm;
using treelilnick.regex;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace treelilnick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage? queryImage;
        // private BitmapImage? resultImage;
        public MainWindow()
        {
            queryImage = null;
            InitializeComponent();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isKMP = algoTypeToggle.IsChecked;
                listView.Items.Clear();
                if (queryImage == null)
                {
                    MessageBox.Show("Please insert your query image");
                    return;
                }
                // connect to database
                Database conn = new Database();
                List<Pair<string, string>> pairs = conn.GetDataSidikJari();

                // start the algorithm
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                BitmapSource queryGrayScale = ImagePreproccess.ConvertToGrayscale(queryImage);
                List<string> queryAscii = ImagePreproccess.ConvertToASCII(queryGrayScale);
                string queryAsciiString = String.Join("", queryAscii);
                string midQueryInit = queryAscii[queryAscii.Count / 2];
                int queryAsciiWidth = midQueryInit.Length;
                StringBuilder temp = new();
                int i = 0;

                for (; i < queryAsciiWidth - 4; i++)
                {
                    if (midQueryInit[i] != 0) break;
                }

                for (int j = 0; j < 4; j++)
                {
                    temp.Append(midQueryInit[i + j]);
                }

                string pattern = temp.ToString();
                bool found = false;
                int indexFound = -1;
                double maxPercentage = 0;

                for (i = 0; i < pairs.Count; i++)
                {
                    string imagePath = pairs[i].First;
                    imagePath = "../test/" + imagePath;
                    BitmapSource imageBitmap = ImageLoader.LoadImage(imagePath);
                    BitmapSource imageGrayScale = ImagePreproccess.ConvertToGrayscale(imageBitmap);
                    string text = ImagePreproccess.ConvertToASCIIString(imageGrayScale);
                    if (isKMP.HasValue && (bool)isKMP)
                    {
                        found = KMP.FindPattern(text, pattern);
                    }
                    else
                    {
                        found = BayerMoore.FindPattern(text, pattern);
                    }
                    if (found)
                    {
                        indexFound = i;
                        maxPercentage = 100;
                        break;
                    }
                }

                if (!found)
                {
                    for (i = 0; i < pairs.Count; i++)
                    {
                        string imagePath = pairs[i].First;
                        imagePath = "../test/" + imagePath;
                        BitmapSource imageBitmap = ImageLoader.LoadImage(imagePath);
                        BitmapSource imageGrayScale = ImagePreproccess.ConvertToGrayscale(imageBitmap);
                        string text = ImagePreproccess.ConvertToASCIIString(imageGrayScale);
                        int hammingDist = ClosestString.HammingDistance(text, queryAsciiString);
                        double percentage = (1 - (hammingDist / (double)Math.Min(text.Length, queryAsciiString.Length))) * 100;
                        if (percentage > maxPercentage)
                        {
                            maxPercentage = percentage;
                            indexFound = i;
                        }
                    }
                }
                // end of algorithm
                stopwatch.Stop();
                long time = stopwatch.ElapsedMilliseconds;

                outputImage.Source = ImageLoader.LoadImage("../test/" + pairs[indexFound].First);
                timeLabel.Text = "Waktu Pencarian: " + time.ToString() +"ms";
                percentageLabel.Text = "Persentase Kecocokan: " + maxPercentage.ToString("F3") + "%";

                List<Pair<string, string>> listAlay = conn.GetNamaAndNIK();
                Pair<string, string> matched;
                try
                {
                    matched = AlayRegex.SearchAlay(pairs[indexFound].Second, listAlay);
                    List<string> data = conn.GetBioData(matched.Second);
                    foreach(string s in data)
                    {
                        listView.Items.Add(s);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //MessageBox.Show("status: " + isKMP.ToString());
            //string currentDirectory = Environment.CurrentDirectory;

            // Show the current directory in a message box
            //MessageBox.Show("Current Directory: " + currentDirectory
            // if (isKMP.HasValue && (bool)isKMP)
            // {
            //     timeLabel.Text = "Checked";
            // }
            // else
            // {
            //     timeLabel.Text = "Unchecked";
            // }
            // Database conn = new Database();
            //List<Pair<string, string>> pairs = conn.GetNamaAndNIK();
            //foreach (Pair<string,string> pair in pairs)
            //{
            //    listView.Items.Add(pair.First + " : " + pair.Second);
            //}
            //List<Pair<string, string>> listAlay = conn.GetNamaAndNIK();
            //Pair<string, string> matched;
            // List<Pair<string, string>> pragma = conn.GetDataSidikJari();
            // foreach(Pair<string, string> pair in pragma)
            // {
            //     listView.Items.Add(pair.Second + " : " + pair.First);
            // }
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
                queryImage = new BitmapImage(new Uri(openFileDialog.FileName));
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