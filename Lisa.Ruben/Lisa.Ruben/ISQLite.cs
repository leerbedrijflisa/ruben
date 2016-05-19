using System;
using SQLite;

namespace Lisa.Ruben
{
	public interface ISQLite  
	{  
		SQLiteConnection GetConnection();  
	}  
}