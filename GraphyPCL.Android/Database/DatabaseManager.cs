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
        public SQLiteConnection GetConnection()
        {
            var dbName = "graphy.db";
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documentsDirectory, dbName);

            // ## Delete if exist (For test)
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }

            var dbConnection = new SQLiteConnection(new SQLitePlatformAndroid(), dbPath);
            return dbConnection;
        }
    }
}

