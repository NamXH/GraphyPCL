using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using GraphyPCL.Android;

[assembly: Dependency(typeof(DatabaseManagerAndroid))]
namespace GraphyPCL.Android
{
    public class DatabaseManagerAndroid : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "graphy.db";
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(documentPath, dbName);

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

