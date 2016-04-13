using Xamarin.Forms;  
using Lisa.Ruben.WinPhone;  
using System.IO;  
using Windows.Storage;
using SQLite;

[assembly: Dependency(typeof(SQLite_WinPhone))]  
namespace Lisa.Ruben.WinPhone  
{  
	public class SQLite_WinPhone : ISQLite  
	{  

		public SQLiteConnection GetConnection()  
		{  
			var filename = "Student.db3";  
			var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);

            //var platfrom = new SQLite.Net.Platform.WindowsPhone8.SQLitePlatformWP8();  
            var connection = new SQLiteConnection(path);
            return (SQLiteConnection)connection;
        }  
	}  
}  