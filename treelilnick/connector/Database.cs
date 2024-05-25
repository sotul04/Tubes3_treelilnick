using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace treelilnick.connector
{
    public class Database
    {
        private static SQLiteConnection connection;

        public static SQLiteConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        static Database()
        {
            connection = new SQLiteConnection("Data Source=SidikJari.db;Version=3;");
            connection.Open();

            Application.Current.Exit += (sender, e) => connection.Close();
        }

        public string GetName()
        {
            return "Connector";
        }

        // List<Pair<string, string>>
        public List<Pair<string, string>> GetDataSidikJari()
        {
            // Berkas citra : First, nama : Second
            List<Pair<string, string>> data = new List<Pair<string, string>>();

            using (SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM sidik_jari", Connection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new Pair<string, string>(reader["berkas_citra"].ToString(), reader["nama"].ToString()));
                    }
                }
            }

            if (data.Count > 0)
            {
                MessageBox.Show(data[0].First + " " + data[0].Second);
            }

            return data;
        }

        public List<Pair<string,string>> GetNamaAndNIK()
        {
            // Nama: First, NIK : Second;
            List<Pair<string, string>> pairs = new List<Pair<string, string>>();
            using (SQLiteCommand selectCommand = new SQLiteCommand("SELECT nama,NIK FROM biodata", Connection))
            {
                using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pairs.Add(new Pair<string, string>(reader["nama"].ToString(), reader["NIK"].ToString()));
                    }
                }
            }
            return pairs;
        }

        public List<string> GetBioData(string nik)
        {
            List<string> biodata = new List<string>();
            try
            {
                using (SQLiteCommand selectCommand = new SQLiteCommand("SELECT * FROM biodata WHERE NIK=@nik", Connection))
                {
                    selectCommand.Parameters.AddWithValue("@nik", nik);

                    using (SQLiteDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            biodata.Add(reader["NIK"].ToString());
                            biodata.Add(reader["nama"].ToString());
                            biodata.Add(reader["tempat_lahir"].ToString());
                            biodata.Add(reader["tanggal_lahir"].ToString().Substring(0,10));
                            biodata.Add(reader["jenis_kelamin"].ToString());
                            biodata.Add(reader["golongan_darah"].ToString());
                            biodata.Add(reader["alamat"].ToString());
                            biodata.Add(reader["agama"].ToString());
                            biodata.Add(reader["status_perkawinan"].ToString());
                            biodata.Add(reader["pekerjaan"].ToString());
                            biodata.Add(reader["kewarganegaraan"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving biodata: " + ex.Message);
            }

            return biodata;
        }

    }

    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }

        public Pair(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}
