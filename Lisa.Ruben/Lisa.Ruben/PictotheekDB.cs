using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace Lisa.Ruben
{
	public class PictotheekDB
	{
		private SQLiteConnection _sqlconnection;  

		public PictotheekDB ()
		{
			//Getting conection and Creating table  
			_sqlconnection = DependencyService.Get<ISQLite>().GetConnection();  
			_sqlconnection.CreateTable<Picto>(); 
		}

		//Get all pictos  
		public IEnumerable<Picto> GetAllPictos()  
		{  
			return (from t in _sqlconnection.Table<Picto>() select t).ToList();  
		}  

		//Get specific picto
		public Picto GetPicto(int id)  
		{  
			return _sqlconnection.Table<Picto>().FirstOrDefault(t => t.Id == id);  
		}  

		//Delete specific picto  
		public void DeletePicto(int id)  
		{  
			_sqlconnection.Delete<Picto>(id);  
		}  

		//Delete all pictos  
		public void DeleteAllPictos()  
		{  
			_sqlconnection.DeleteAll<Picto> (); 
		} 

		//Add new picto to DB  
		public void AddPicto(Picto picto)  
		{  
			_sqlconnection.Insert(picto);  
		}  

		public bool CheckLabelNameExists(string name)
		{
			var results = _sqlconnection.Query<Picto> ("SELECT * FROM Picto WHERE label = ?",name);
			if (results.Count > 0) 
			{
				return true;
			}
			return false;
		}

		public int GetIdFromName(string name)
		{
			var results = _sqlconnection.Query<Picto> ("SELECT * FROM Picto WHERE label = ?",name);
			if (results.Count > 0) 
			{
				return results[0].Id;
			}
			return 0;
		}
	}
}