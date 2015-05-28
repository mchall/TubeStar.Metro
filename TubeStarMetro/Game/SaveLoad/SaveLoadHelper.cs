using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TubeStarMetro
{
    public static class SaveLoadHelper
    {
        public static string SaveFile
        {
            get { return "TubeStarSave.tss"; }
        }

        public static async void Save(string fileName)
        {
            try
            {
                SaveObj obj = new SaveObj();
                obj.Player = Player.Current;
                obj.ShareViews = VideoViewer.GetShares();
                obj.BoughtViews = VideoViewer.GetBoughtViews();
                obj.Studies = Studies.Current;
                obj.StoreItems = StoreItems.Current;

                string xml;
                using (MemoryStream ms = new MemoryStream())
                {
                    var ser = new DataContractSerializer(obj.GetType());
                    ser.WriteObject(ms, obj);

                    xml = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                    var encryptedXml = EncryptionHelpers.Encrypt(xml);
                    
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(SaveFile, CreationCollisionOption.ReplaceExisting);
                    await Windows.Storage.FileIO.WriteTextAsync(file, encryptedXml);
                }
            }
            catch (Exception)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.Failure.Translate(), EnglishStrings.SaveFail.Translate(), MessagePicture.Error);
            }
        }

        public static async Task Load(string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(SaveFile);
                var encryptedXml = await Windows.Storage.FileIO.ReadTextAsync(file);
                var xml = EncryptionHelpers.Decrypt(encryptedXml);
                var bytes = System.Text.Encoding.UTF8.GetBytes(xml);

                SaveObj saveObj;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    var ser = new DataContractSerializer(typeof(SaveObj));
                    saveObj = (SaveObj)ser.ReadObject(ms);
                }

                Player.Current.Merge(saveObj.Player);

                Player.Current.Internet.ClearUsers();
                foreach (var channel in Player.Current.Channels)
                {
                    Player.Current.Internet.SetUsers(channel.Subscribers);
                }

                VideoViewer.SetShares(saveObj.ShareViews);
                VideoViewer.SetBoughtViews(saveObj.BoughtViews);
                Studies.Current = saveObj.Studies;
                StoreItems.Current = saveObj.StoreItems;
            }
            catch (Exception)
            {
                CustomMessageBox.ShowDialog(EnglishStrings.Failure.Translate(), EnglishStrings.LoadFail.Translate(), MessagePicture.Error);
            }
        }

        public async static Task<bool> SaveExistsAsync()
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            return files.Any((f) =>
            {
                return f.Name == SaveLoadHelper.SaveFile;
            });
        }
    }
}