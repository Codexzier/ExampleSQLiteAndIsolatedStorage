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
        
        public ComparePerformanceView()
        {
            this.InitializeComponent();

            this.DataContext = this._viewModel;

            // set default start values
            this._viewModel.WriteDataCount = 10;
            this._viewModel.Status = "Nothing";
            this._viewModel.CountOfIterations = 3;
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

            if (this._viewModel.WriteDataCount < 1)
            {
                throw new Exception("Wert muss größer sein als 0.");
            }

            this._viewModel.Results.Clear();

            TestRunEnvironment testRunEnvironment = new TestRunEnvironment();
            var result = await testRunEnvironment.RunTests(this._viewModel.WriteDataCount, this._viewModel.CountOfIterations);



            for (int i = 0; i < this._viewModel.CountOfIterations; i++)
            {
                this.AddResult((i + 1).ToString(), i, result.ResultSQLiteTests, result.ResultsIsolatedStoargeTests);
            }
            this.AddResult("Average", this._viewModel.CountOfIterations, result.ResultSQLiteTests, result.ResultsIsolatedStoargeTests);

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
                IsolatedStorageWrite = isolatedStorageResults.ResultsSaved[index].Item2,
                IsolatedStorageRead = isolatedStorageResults.ResultsRead[index].Item2,
                IsolatedStorageReadWhere = isolatedStorageResults.ResultsReadWhere[index].Item2
            };
            this._viewModel.Results.Add(resultDataAverage);
        }
    }
}
