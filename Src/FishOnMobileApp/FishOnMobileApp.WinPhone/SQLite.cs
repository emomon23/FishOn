using System;
using System.IO;
using Windows.Storage;
using FishOn.PlatformInterfaces;
using Xamarin.Forms;
using SQLite.Net.Async;
using SQLite.Net;


[assembly: Dependency(typeof(FishOnMobileApp.WinPhone.SQLite))]
namespace FishOnMobileApp.WinPhone
{
    public class SQLite : ISQLite
    {
        private SQLiteConnectionWithLock _conn;

        public SQLite()
        {

        }

        private static string GetDatabasePath()
        {
            const string sqliteFilename = "FishOnMobile.db";

            string documentsPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }

        public bool DoesDBExist
        {
            get { return false; }
        }

        public SQLiteConnection GetConnection()
        {

            var dbPath = GetDatabasePath();

            // Return the synchronous database connection 
            return null; // new SQLiteConnection()
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

          
            var connectionFactory = new Func<SQLiteConnectionWithLock>(
                () =>
                {
                    if (_conn == null)
                    {
                       // _conn = new SQLiteConnectionWithLock(platForm,new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: true));
                    }
                    return _conn;
                });

            return new SQLiteAsyncConnection(connectionFactory);
        }

        public void DeleteDatabase()
        {
            var path = GetDatabasePath();

            try
            {
                if (_conn != null)
                {
                    _conn.Close();

                }
            }
            catch
            {
                // Best effort close. No need to worry if throws an exception
            }

          
            _conn = null;
        }

        public void CloseConnection()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn.Dispose();
                _conn = null;

                // Must be called as the disposal of the connection is not released until the GC runs.
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}