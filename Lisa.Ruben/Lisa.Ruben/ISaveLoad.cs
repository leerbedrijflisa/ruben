using System;
using System.Threading.Tasks;

namespace Lisa.Ruben
{
	public interface ISaveLoad
	{
		Task SaveTextAsync (string filename, string text);
		Task<string> LoadTextAsync (string filename);
		bool FileExists (string filename);
	}
}