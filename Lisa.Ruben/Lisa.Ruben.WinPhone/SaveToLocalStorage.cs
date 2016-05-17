using Lisa.Ruben.WinPhone;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveToLocalStorage))]
namespace Lisa.Ruben.WinPhone
{
    public class SaveToLocalStorage : ISaveToLocalStorage
    {
        public async Task SaveToLocalFolderAsync(Stream file, string fileName)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            System.Diagnostics.Debug.WriteLine("----------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine(localFolder);

            using (Stream outputStream = await storageFile.OpenStreamForWriteAsync())
            {
                await file.CopyToAsync(outputStream);
            }
        }

        public string GetPath()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return localFolder.Path;
        }
    }
}
