using System;
using System.Linq;
using Windows;
using Windows.Storage;

namespace UITestApp.AppCode
{
    public class FileAccessAppCode
    {
        public static string pth;

        public static async void ReadExternalSqliteFile()
        {

            // Get the logical root folder for all external storage devices.
            StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;
            // Get the first child folder, which represents the SD card.
            StorageFolder sdCard = (await externalDevices.GetFoldersAsync()).FirstOrDefault();
            pth = sdCard.Path;
            // An SD card is present and the sdCard variable now contains a to reference it.
            //if (sdCard != null)
            //{
            //    StorageFolder resultFolder = await sdCard.CreateFolderAsync("AliAtaBak",CreationCollisionOption.OpenIfExists);
            //    StorageFile resultfile = await sdCard.CreateFileAsync("foo.txt");
            //    string base64 = "/9j/4AAQSkZJRgABAQEAYABgAAD/4RjqR....";
            //     var bytes = Convert.FromBase64String(base64);
            //    await FileIO.WriteBytesAsync(resultfile, bytes);
            //}
            //// No SD card is present.
            //else
            //{
            //}
        }
    }
}
