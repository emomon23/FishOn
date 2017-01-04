using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;

namespace FishOn.PlatformInterfaces
{
    /// <summary>
    /// SQLite.Net-PCL
    /// 
    /// https://github.com/oysteinkrog/SQLite.Net-PCL
    /// http://www.xamarinhelp.com/local-storage-day-10/
    /// </summary>
    public interface ISQLite
    {
        void CloseConnection();
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
        void DeleteDatabase();
        bool DoesDBExist { get; }
    }
}
