using ExampleSQLiteAndIsolatedStorage.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    /// <summary>
    /// A simple class with like same function use for compare testing with sqlite.
    /// The clas can read all data saved, add new data and clear all data.
    /// </summary>
    public class IsolatedStorageManagerTestData
    {
        private string _filename = "testRunFile.csv";

        /// <summary>
        /// Load saved data from isolatedStoarge. if the file not exist, then it create a new csv file with the name 'testRunFile.csv'.
        /// </summary>
        /// <returns>Return a list of saved data. If not, then return a list with 0 items.</returns>
        public async Task<List<TestData>> Load()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

            var result = await folder.GetFilesAsync();

            if (!result.Any(f => f.Name == this._filename))
            {
                StorageFile file = await folder.CreateFileAsync(this._filename);
                return new List<TestData>();
            }

            return await this.LoadData();
        }

        /// <summary>
        /// Read the data from the csv file and parse all value to target object and add to the list.
        /// </summary>
        /// <returns>Return the list of parsed data.</returns>
        private async Task<List<TestData>> LoadData()
        {
            StorageFile file = await this.GetLocalStorage();
            string loadData = await FileIO.ReadTextAsync(file);

            List<TestData> list = new List<TestData>();
            foreach (string item in loadData.Split('\r', '\n'))
            {
                TestData data = new TestData();
                string[] sa = item.Split(';');

                if (sa.Length < 5)
                {
                    continue;
                }

                if (int.TryParse(sa[0], out int id))
                {
                    data.Id = id;
                }

                if(int.TryParse(sa[1], out int temperatur))
                {
                    data.Temperatur = temperatur;
                }

                if(double.TryParse(sa[2], out double pressure))
                {
                    data.Pressure = pressure;
                }

                if(float.TryParse(sa[3], out float humidity))
                {
                    data.Humidity = humidity;
                }

                if(DateTime.TryParse(sa[4], out DateTime time))
                {
                    data.Time = time;
                }

                data.Room = sa[5];

                list.Add(data);
            }

            return list;
        }

        /// <summary>
        /// Add the data to the CSV file.
        /// </summary>
        /// <param name="data">Data to save</param>
        /// <returns>Return Task for await.</returns>
        public async Task Add(TestData data)
        {
            StorageFile file = await this.GetLocalStorage();
            string loadData = await FileIO.ReadTextAsync(file);
            StringBuilder sb = new StringBuilder(loadData);

            sb.AppendLine($"{data.Id};{data.Temperatur};{data.Pressure};{data.Humidity};{data.Time};{data.Room}");

            await FileIO.WriteTextAsync(file, sb.ToString());
        }

        /// <summary>
        /// Clear all data saved in the csv file.
        /// </summary>
        /// <returns></returns>
        internal async Task Clear()
        {
            StorageFile file = await this.GetLocalStorage();
            await FileIO.WriteTextAsync(file, string.Empty);
        }

        /// <summary>
        /// Get the local file from isolate storaged.
        /// </summary>
        /// <returns></returns>
        internal async Task<StorageFile> GetLocalStorage()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await folder.GetFileAsync(this._filename);
        }
    }
}
