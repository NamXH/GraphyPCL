using System;
using System.IO;
using GraphyPCL;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(GraphyPCL.iOS.DatabaseManager))]
namespace GraphyPCL.iOS
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
            return new SQLiteConnection(new SQLitePlatformIOS(), DbPath);
        }

        public bool Exists()
        {
            return File.Exists(DbPath);
        }
    }
}