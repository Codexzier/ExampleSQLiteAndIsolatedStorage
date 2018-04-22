using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExampleSQLiteAndIsolatedStorage.Views.WithIsolatedStorage
{
    /// <summary>
    /// Example class for an easy way to used isolated storage to save data in a csv file.
    /// </summary>
    public class IsolatedStorageManager
    {
        private string _filename = "myFile.csv";

        public async Task<List<MyData>> Load()
        {
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;

                var result = await folder.GetFilesAsync();
                foreach (var item in result)
                {

                }
                if (!result.Any(f => f.Name == this._filename))
                {
                    StorageFile file = await folder.CreateFileAsync(this._filename);
                    return new List<MyData>();
                }

               return  await this.LoadData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new List<MyData>();
        }

        private async Task<List<MyData>> LoadData()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(this._filename);
            string loadData = await FileIO.ReadTextAsync(file);

            List<MyData> list = new List<MyData>();
            foreach (string item in loadData.Split('\r', '\n'))
            {
                MyData data = new MyData();
                string[] sa = item.Split(';');

                if (sa.Length < 2)
                {
                    continue;
                }

                if (int.TryParse(sa[0], out int id))
                {
                    data.Id = id;
                }

                data.Content = sa[1];

                list.Add(data);
            }

            return list;
        }

        public async Task Save(MyData data)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(this._filename);
            string loadData = await FileIO.ReadTextAsync(file);
            StringBuilder sb = new StringBuilder();
            int lastId = 1;
            foreach (string item in loadData.Split('\r', '\n'))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                string[] sa = item.Split(';');
                if (sa.Length < 2)
                {
                    continue;
                }
                if (int.TryParse(sa[0], out int id))
                {
                    lastId = id;
                }

                sb.AppendLine(item);
            }
            lastId++;
            sb.AppendLine($"{lastId};{data.Content}");

            await FileIO.WriteTextAsync(file, sb.ToString());
        }

        internal async Task Clear()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(this._filename);
            await FileIO.WriteTextAsync(file, string.Empty);
        }
    }
}
