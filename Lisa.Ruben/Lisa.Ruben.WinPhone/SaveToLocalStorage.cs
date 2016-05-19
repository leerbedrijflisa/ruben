using Lisa.Ruben.WinPhone;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveToLocalStorage))]
namespace Lisa.Ruben.WinPhone
{
    public class SaveToLocalStorage : ISaveToLocalStorage
    {
        public async Task SaveToLocalFolderAsync(Stream file, string fileName)
        {
            fileName = fileName + ".jpg";

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            System.Diagnostics.Debug.WriteLine("----------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine(localFolder);

            Stream outputStream = await storageFile.OpenStreamForWriteAsync();

            file.Seek(0, SeekOrigin.Begin);
            outputStream.Seek(0, SeekOrigin.Begin);
            await file.CopyToAsync(outputStream);
        }

        public string GetPath()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return localFolder.Path;
        }
    }
}
