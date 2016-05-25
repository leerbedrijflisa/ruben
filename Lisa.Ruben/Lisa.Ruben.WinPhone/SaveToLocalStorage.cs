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
            //add mime type to filename
            fileName = fileName + ".jpg";

            //find the localstorage and create an empty file
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            Stream outputStream = await storageFile.OpenStreamForWriteAsync();

            //set streams to the beginning
            file.Seek(0, SeekOrigin.Begin);
            outputStream.Seek(0, SeekOrigin.Begin);

            //copy the stream into the file
            await file.CopyToAsync(outputStream);
        }

        //returns the localfolder as a string
        public string GetPath()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return localFolder.Path;
        }

        public async void UpdateFileName(string oldName, string newName)
        {
            oldName += ".jpg";
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.GetFileAsync(oldName);
            newName = newName + ".jpg";
            await storageFile.RenameAsync(newName);
        }
    }
}