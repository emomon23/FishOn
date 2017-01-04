using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using System;
using System.IO;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.Droid;
using Xamarin.Forms;
using SQLite.Net.Async;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;

[assembly: Dependency(typeof(FishOnMobileApp.Droid.SQLite))]
namespace FishOnMobileApp.Droid
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

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }

        public bool DoesDBExist => File.Exists(GetDatabasePath());

        public SQLiteConnection GetConnection()
        {
            var dbPath = GetDatabasePath();

            // Return the synchronous database connection 
            return new SQLiteConnection(new SQLitePlatformAndroid(), dbPath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            var platForm = new SQLitePlatformAndroid();

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

            if (File.Exists(path))
            {

                File.Delete(path);
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