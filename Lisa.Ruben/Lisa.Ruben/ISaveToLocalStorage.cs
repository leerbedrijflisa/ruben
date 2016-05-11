using System.IO;
using System.Threading.Tasks;

namespace Lisa.Ruben
{
    interface ISaveToLocalStorage
    {
        Task SaveToLocalFolderAsync(Stream file, string fileName);
    }
}