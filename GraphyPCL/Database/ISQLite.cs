using System;
using SQLite.Net;

namespace GraphyPCL
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();

        bool Exists();

        void Delete();
    }
}