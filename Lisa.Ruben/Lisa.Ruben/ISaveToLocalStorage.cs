using System.IO;
using System.Threading.Tasks;

namespace Lisa.Ruben
{
   public interface ISaveToLocalStorage
    {
        Task SaveToLocalFolderAsync(Stream file, string fileName);
        string GetPath();
    }
}