﻿using System;
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
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                        data.Add(new Pair<string, string>(reader["berkas_citra"].ToString(), reader["nama"].ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }

            // if (data.Count > 0)
            // {
            //     MessageBox.Show(data[0].First + " " + data[0].Second);
            // }

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
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                        pairs.Add(new Pair<string, string>(reader["nama"].ToString(), reader["NIK"].ToString()));
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
            }
            return pairs;
        }

        public static int NIK = 0;
        public static int NAMA = 1;
        public static int TEMPAT_LAHIR = 2;
        public static int TANGGAL_LAHIR = 3;
        public static int JENIS_KELAMIN = 4;
        public static int GOLONGAN_DARAH = 5;
        public static int ALAMAT = 6;
        public static int AGAMA = 7;
        public static int STATUS_PERKAWINAN = 8;
        public static int PEKERJAAN = 9;
        public static int KEWARGANEGARAAN = 10;

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
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["NIK"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["nama"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["tempat_lahir"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            biodata.Add(reader["tanggal_lahir"].ToString().Substring(0,10));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["jenis_kelamin"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["golongan_darah"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["alamat"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["agama"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["status_perkawinan"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["pekerjaan"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                            biodata.Add(reader["kewarganegaraan"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
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
