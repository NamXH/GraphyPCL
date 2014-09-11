using System;
using System.IO;
using GraphyPCL;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;
using GraphyPCL.iOS;

[assembly: Dependency(typeof(DatabaseManageriOS))]
namespace GraphyPCL.iOS
{
    public class DatabaseManageriOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "graphy.db";
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentPath, "..", "Library");
            var dbPath = Path.Combine(libraryPath, dbName);

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