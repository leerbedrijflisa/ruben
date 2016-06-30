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

        //renames an already existing picto, oldname is used to find the picto
        public async void UpdateFileName(string oldName, string newName)
        {
            oldName += ".jpg";
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.GetFileAsync(oldName);
            newName = newName + ".jpg";
            string path = storageFile.Path;
            File.SetAttributes(path, System.IO.FileAttributes.Normal);
            await storageFile.RenameAsync(newName);
        }

        public async Task<string> WriteStreamToFile(Stream stream)
        {
            byte[] fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("file.jpg", CreationCollisionOption.GenerateUniqueName);
            File.SetAttributes(sampleFile.Path, System.IO.FileAttributes.Normal);
            using (Stream file = File.OpenWrite(sampleFile.Path))
            {
                file.Write(fileBytes, 0, fileBytes.Length);
            }
            return sampleFile.Path;
        }
    }
}