using System;
using System.IO;
using Xamarin.Forms;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using System.Diagnostics;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.iOS;


[assembly: Dependency(typeof(FishOnMobileApp.iOS.SQLite))]
namespace FishOnMobileApp.iOS
{
    public class SQLite : ISQLite
    {
        private SQLiteConnectionWithLock _conn;

        public SQLite()
        {

        }

        private static string GetDatabasePath()
        {
            const string sqliteFilename = "mydatabase.db3";

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var pathFull = Path.Combine(libraryPath, sqliteFilename);

            // It's useful to know this, so you can explore the database with something like SQLiteBrowser
            Debug.WriteLine(pathFull);

            return pathFull;
        }

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            // Return the synchronous database connection 
            return new SQLiteConnection(new SQLitePlatformIOS(), dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            var platForm = new SQLitePlatformIOS();

            var connectionFactory = new Func<SQLiteConnectionWithLock>(
                () =>
                {
                    if (_conn == null)
                    {
                        _conn =
                            new SQLiteConnectionWithLock(platForm,
                                new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: true));
                    }
                    return _conn;
                });
            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);

            return asyncConnection;
        }

        public bool DoesDBExist => File.Exists(GetDatabasePath());

        public void DeleteDatabase()
        {
            try
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

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                _conn = null;

            }
            catch
            {
                throw;
            }
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

