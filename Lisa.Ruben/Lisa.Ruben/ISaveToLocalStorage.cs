using System.IO;
using System.Threading.Tasks;

namespace Lisa.Ruben
{
    //this handles the platform specific implementation of save to localstorage
   public interface ISaveToLocalStorage
    {
        Task SaveToLocalFolderAsync(Stream file, string fileName);
        string GetPath();
        void UpdateFileName(string oldName, string newName);

        Task<string> WriteStreamToFile(Stream stream);
    }
}