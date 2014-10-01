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

            var dbConnection = new SQLiteConnection(new SQLitePlatformIOS(), dbPath);
            return dbConnection;
        }
    }
}