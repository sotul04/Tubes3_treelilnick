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
using treelilnick.decryptor;
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
                string pattern = ImagePreproccess.ConvertToASCIIPattern(queryGrayScale);
                bool found = false;
                int indexFound = -1;
                double maxPercentage = 0;
                int i = 0;

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
                    string queryAsciiString = ImagePreproccess.ConvertToASCIIString(queryGrayScale);
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
                    listView.Items.Add("NIK:\t\t\t"+data[Database.NIK]);
                    listView.Items.Add("Nama:\t\t\t"+pairs[indexFound].Second);
                    listView.Items.Add("Tempat Lahir:\t\t"+Decrypt.DecryptCipher(data[Database.TEMPAT_LAHIR]));
                    listView.Items.Add("Tanggal Lahir:\t\t"+data[Database.TANGGAL_LAHIR]);
                    listView.Items.Add("Jenis Kelamin:\t\t"+data[Database.JENIS_KELAMIN]);
                    listView.Items.Add("Golongan Darah:\t\t"+data[Database.GOLONGAN_DARAH]);
                    listView.Items.Add("Alamat:\t\t\t"+Decrypt.DecryptCipher(data[Database.ALAMAT]));
                    listView.Items.Add("Agama:\t\t\t"+Decrypt.DecryptCipher(data[Database.AGAMA]));
                    listView.Items.Add("Status Perkawinan:\t"+data[Database.STATUS_PERKAWINAN]);
                    listView.Items.Add("Pekerjaan:\t\t"+Decrypt.DecryptCipher(data[Database.PEKERJAAN]));
                    listView.Items.Add("Kewarganegaraan:\t"+Decrypt.DecryptCipher(data[Database.KEWARGANEGARAAN]));
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
    }
}