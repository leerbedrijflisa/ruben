using System;  
using System.IO;  
using Xamarin.Forms;  
using Lisa.Ruben.Droid;
using SQLite;

[assembly: Dependency(typeof(SQLite_Android))]  
namespace Lisa.Ruben.Droid  
{  
	public class SQLite_Android: ISQLite  
	{  
		public SQLiteConnection GetConnection()  
		{  
			var filename = "Picto.db3";  
			var documentspath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);  
			var path = Path.Combine(documentspath, filename);  

			//var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();  
			var connection = new SQLiteConnection(path);  
			return (SQLiteConnection)connection;  
		}  
	}  
} 