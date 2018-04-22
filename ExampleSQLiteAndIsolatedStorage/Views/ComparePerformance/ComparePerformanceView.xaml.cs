using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    public sealed partial class ComparePerformanceView : UserControl
    {
        private ComparePerformanceViewModel _viewModel = new ComparePerformanceViewModel();

        /// <summary>
        /// Main setup for count of iteration of a target funktion test.
        /// </summary>
        private int _countOfIterations = 10;

        public ComparePerformanceView()
        {
            this.InitializeComponent();

            this.DataContext = this._viewModel;

            // set default start values
            this._viewModel.WriteDataCount = 10;
            this._viewModel.Status = "Nothing";
            
        }

        /// <summary>
        /// Execute by clicking the 'Run' button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this._viewModel.Status = "Is running...";

            await Task.Delay(500);

            if(this._viewModel.WriteDataCount < 1)
            {
                throw new Exception("Wert muss größer sein als 0.");
            }
            
            // clear storage and results
            await this.DeleteFile("testRunFile.csv");
            await this.DeleteFile("testDatabase.db");
            this._viewModel.Results.Clear();
            
            // test runs with SQLite
           var resultSQLiteTests = await this.StartTestRuns(this.GetInitSQLiteConnectionAndCreateTable(), 
                                                               this.GetFuncIterationSQLiteInsert(), 
                                                               this.GetFuncSQLiteRead(), 
                                                               this.GetFuncSQLiteReadWithWhere());

            // test runs with isolated stoage
            var resultsIsolatedStoargeTests = await this.StartTestRuns(this.GetIniIsolatedStoargeAndCreateFile(),
                                                                        this.GetFuncIterationIsolatedStorageSave(),
                                                                        this.GetFuncIsolatedStorageRead(),
                                                                        this.GetFuncIsolatedStorageReadAndWhere());

            for (int i = 0; i < this._countOfIterations; i++)
            {
                this.AddResult((i + 1).ToString(), i, resultSQLiteTests, resultsIsolatedStoargeTests);
            }
            this.AddResult("Average", this._countOfIterations, resultSQLiteTests, resultsIsolatedStoargeTests);

            this._viewModel.Status = "Finish";
        }

        private void AddResult(string rowTitle, int index, TestResults sqliteResults, TestResults isolatedStorageResults)
        {
            var resultDataAverage = new ListViewItemResult
            {
                LeftRowTitle = rowTitle,
                SQLiteResultWrite = sqliteResults.ResultsSaved[index].Item2,
                SQLiteResultRead = sqliteResults.ResultsRead[index].Item2,
                SQLiteResultReadWhere = sqliteResults.ResultsReadWhere[index].Item2,
                IsolatedStorageWrite = sqliteResults.ResultsSaved[index].Item2,
                IsolatedStorageRead = sqliteResults.ResultsRead[index].Item2,
                IsolatedStorageReadWhere = sqliteResults.ResultsReadWhere[index].Item2
            };
            this._viewModel.Results.Add(resultDataAverage);
        }
        
        /// <summary>
        /// Delete the file from app storage.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private async Task DeleteFile(string filename)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;

           var exist = (await  folder.GetFilesAsync()).ToList();

            if (exist.Any(a => a.Name.Contains(filename)))
            {
                StorageFile file1 = await folder.GetFileAsync(filename);
                await file1.DeleteAsync();
            }
        }

        #region SQLite functions

        /// <summary>
        /// Initialize the SQLite connection and create a table for 'TestData'.
        /// </summary>
        /// <returns>Return the SQLite connection.</returns>
        private Func<Task<SQLiteConnection>> GetInitSQLiteConnectionAndCreateTable()
        {
            return () =>
            {
                string path = $"{ApplicationData.Current.LocalFolder.Path}\\testDatabase.db";
                var connection = new SQLiteConnection(new SQLitePlatformWinRT(), path);

                // create the target table to insert the data.
                connection.CreateTable<TestData>();
                return Task.FromResult(connection);
            };
        }
        
        /// <summary>
        /// function to add a data to the SQLite database.
        /// </summary>
        /// <returns>(Unused)</returns>
        private Func<SQLiteConnection, DateTime, bool, int, Task<bool>> GetFuncIterationSQLiteInsert()
        {
            // last parameter for index is hier not using. it only ussed for isolateStorage
            return (connection, dt, b, i) =>
            {
                connection.Insert(new TestData { Temperatur = 11, Pressure = 1012.6, Humidity = 40.5f, Time = dt, Room = b ? "Wohnzimmer" : "Flur" });
                return Task.FromResult(true);
            };
        }

        /// <summary>
        /// Function to read data from the sqlite database.
        /// </summary>
        /// <returns>(Unused)</returns>
        private Func<SQLiteConnection, DateTime, bool, int, Task<bool>> GetFuncSQLiteRead()
        {
            // the two parameter 'dt' and 'b' is not using for this.
            // it is only to set the same input parameter for the iteration function
            // last parameter for index is hier not using. it only ussed for isolateStorage
            return (connection, dt, b, i) =>
            {
                var data = connection.Table<TestData>();
                var executedRead = data.ToList();
                return Task.FromResult(true);
            };
        }

        /// <summary>
        /// Function to read data from the sqlite database and use the 'Linq' WHERE methode to filter data with information 'Wohnzimmer'.
        /// </summary>
        /// <returns>(Unused)</returns>
        private Func<SQLiteConnection, DateTime, bool, int, Task<bool>> GetFuncSQLiteReadWithWhere()
        {
            // the two parameter 'dt' and 'b' is not using for this.
            // it is only to set the same input parameter for the iteration function
            // last parameter for index is hier not using. it only ussed for isolateStorage
            return (connection, dt, b, i) =>
            {
                var data = connection.Table<TestData>();
                var executedRead = data.Where(w => w.Room.Contains("Wohnzimmer")).ToList();
                return Task.FromResult(true);
            };
        }

        #endregion

        #region IsolatedStorage

        /// <summary>
        /// Create a new instance of isolatedStorageManager for specifi methode for the test runs.
        /// </summary>
        /// <returns>Return the instance of isolatedStorageManager.</returns>
        private Func<Task<IsolatedStorageManagerTestData>> GetIniIsolatedStoargeAndCreateFile()
        {
            return async () =>
            {
                IsolatedStorageManagerTestData isolatedStorage = new IsolatedStorageManagerTestData();

                // create new file if the file not exist.
                await isolatedStorage.Load();
                return isolatedStorage;
            };
        }

        /// <summary>
        /// Add a data to the isolated storage.
        /// </summary>
        /// <returns>Return the function of executed target method.</returns>
        private Func<IsolatedStorageManagerTestData, DateTime, bool, int, Task<bool>> GetFuncIterationIsolatedStorageSave()
        {
            return async (isolatedStorage, dt, b, i) =>
            {
                await isolatedStorage.Add(new TestData { Id = i, Temperatur = 11, Pressure = 1012.6, Humidity = 40.5f, Time = dt, Room = b ? "Wohnzimmer" : "Flur" });
                return true;
            };
        }

        /// <summary>
        /// Read all data from isolated storage.
        /// </summary>
        /// <returns>(Unused)</returns>
        private Func<IsolatedStorageManagerTestData, DateTime, bool, int, Task<bool>> GetFuncIsolatedStorageRead()
        {
            return async (isolatedStorage, dt, b, i) =>
            {
                var data = await isolatedStorage.Load();
                return true;
            };
        }

        /// <summary>
        /// Read all data from isolated storage and filter the data with containing 'Wohnzimmer'.
        /// </summary>
        /// <returns>(Unused)</returns>
        private Func<IsolatedStorageManagerTestData, DateTime, bool, int, Task<bool>> GetFuncIsolatedStorageReadAndWhere()
        {
            return async (isolatedStorage, dt, b, i) =>
            {
                var data = await isolatedStorage.Load();
                var livingroom = data.Where(w => w.Room.Contains("Wohnzimmer")).ToList();
                return true;
            };
        }

        #endregion

        /// <summary>
        /// Base iteration to run a target execute content and saved the time stop result.
        /// </summary>
        /// <typeparam name="TStorage">Type of using data store.</typeparam>
        /// <param name="storage">Target system of using data store.</param>
        /// <returns>Return the stop watch results.</returns>
        private Func<Func<TStorage, DateTime, bool, int, Task<bool>>, Task<List<Tuple<string, string>>>> GetBaseTimeStopIterationRun<TStorage>(TStorage storage)
        {
            return async (funcTargetFunction) =>
            {
                DateTime dt = DateTime.Now;
                Stopwatch sw = new Stopwatch();
                bool b = true;
                long average = 0;

                List<Tuple<string, string>> results = new List<Tuple<string, string>>();
                for (int iterationSave = 0; iterationSave < this._countOfIterations; iterationSave++)
                {
                    sw.Restart();
                    for (int i = 0; i < this._viewModel.WriteDataCount; i++)
                    {
                        await funcTargetFunction(storage, dt, b, i);
                        b = !b;
                    }
                    sw.Stop();
                    average += sw.ElapsedMilliseconds;
                    results.Add(new Tuple<string, string>(iterationSave.ToString(), $"{sw.ElapsedMilliseconds}ms"));
                }

                var resulToPost = (average / (double)this._countOfIterations);
                results.Add(new Tuple<string, string>("Averagge", $"{resulToPost, 2:N1}ms"));

                return results;
            };
        }
        
        /// <summary>
        /// Main method to run diffrent tests.
        /// </summary>
        /// <typeparam name="TStorage">Type of the using storage.</typeparam>
        /// <param name="funcInit">Set the funtion to initialize the using store and return the instnace.</param>
        /// <param name="funcAsyncSave">Set the function for save data to the target data store.</param>
        /// <param name="funcAsyncRead">Set the function for read data from the target data store.</param>
        /// <param name="funcAsyncReadWhere">Set the function for read and filter from the target data store.</param>
        /// <returns>Return all results from the iterated tests.</returns>
        private async Task<TestResults> StartTestRuns<TStorage>(Func<Task<TStorage>> funcInit, 
                                            Func<TStorage, DateTime, bool, int, Task<bool>> funcAsyncSave, 
                                            Func<TStorage, DateTime, bool, int, Task<bool>> funcAsyncRead,
                                            Func<TStorage, DateTime, bool, int, Task<bool>> funcAsyncReadWhere)
        {
            TStorage storage = await funcInit();

            // save data
            List<Tuple<string, string>> resultsSaved = await this.GetBaseTimeStopIterationRun(storage)(funcAsyncSave);

            // read data
            List<Tuple<string, string>> resultsRead = await this.GetBaseTimeStopIterationRun(storage)(funcAsyncRead);

            // read data with filter
            List<Tuple<string, string>> resultsReadWhere = await this.GetBaseTimeStopIterationRun(storage)(funcAsyncReadWhere);

            return new TestResults(resultsSaved, resultsRead, resultsReadWhere);
        }
    }
}
