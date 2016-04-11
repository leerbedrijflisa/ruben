using System;
using SQLite;

namespace Lisa.Ruben
{
	public class Picto
	{
		[PrimaryKey, AutoIncrement]  
		public int Id { get; set; }  
		public string Path { get; set; }  
		public string Label { get; set; }  

		public Picto ()
		{
		}
	}
}

