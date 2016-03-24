using System;
using Xamarin.Forms;
using Lisa.Ruben.Droid;
using System.IO;
using System.Threading.Tasks;
using Lisa.Ruben;

[assembly: Dependency (typeof (SaveLoad_Android))]

namespace Lisa.Ruben.Droid
{
	public class SaveLoad_Android : ISaveLoad
	{
		#region ISaveLoad implementation

		public async Task SaveTextAsync (string filename, string text)
		{
			var path = CreatePathToFile (filename);
			using (StreamWriter sw = File.CreateText (path))
				await sw.WriteAsync(text);
		}

		public async Task<string> LoadTextAsync (string filename)
		{
			var path = CreatePathToFile (filename);
			using (StreamReader sr = File.OpenText(path))
				return await sr.ReadToEndAsync();
		}

		public bool FileExists (string filename)
		{
			return File.Exists (CreatePathToFile (filename));
		}

		#endregion

		string CreatePathToFile (string filename)
		{
			var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(docsPath, filename);
		}
	}
}