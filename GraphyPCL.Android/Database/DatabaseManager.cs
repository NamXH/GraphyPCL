using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;

[assembly: Dependency(typeof(GraphyPCL.Android.DatabaseManager))]
namespace GraphyPCL.Android
{
    public class DatabaseManager : ISQLite
    {
        private const string c_dbName = "graphy.db";

        public string DbPath
        {
            get
            {
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(documentsDirectory, c_dbName);
            }
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(new SQLitePlatformAndroid(), DbPath);
        }

        public bool Exists()
        {
            return File.Exists(DbPath);
        }
    }
}

