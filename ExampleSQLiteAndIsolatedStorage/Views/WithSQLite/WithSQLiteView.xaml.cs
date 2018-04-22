using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExampleSQLiteAndIsolatedStorage.Views.WithSQLite
{
    public sealed partial class WithSQLiteView : UserControl
    {
        private WithSQLiteViewModel _viewModel = new WithSQLiteViewModel();
        private SQLiteConnection _connection;

        public WithSQLiteView()
        {
            this.InitializeComponent();

            this.DataContext = this._viewModel;
            
            this.InitSQLite();
        }

        /// <summary>
        /// Initialize the connection to the sqlite database.
        /// </summary>
        private void InitSQLite()
        {
            string path = $"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\\myDatabase.db";
            this._connection = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            this._connection.CreateTable<MyData>();
            this.LoadData();
        }

        /// <summary>
        /// Execute by clicking the button 'Save' to add data to the sql database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this._connection.Insert(new MyData { Content = this._viewModel.ContentToSave });

            // reload data
            this.LoadData();
        }

        /// <summary>
        /// load all saved data and add to list to show the results.
        /// </summary>
        private void LoadData()
        {
            var loadedData = this._connection.Table<MyData>();
            foreach (MyData data in loadedData)
            {
                if (!this._viewModel.SavedContents.Any(a => a.Id == data.Id))
                {
                    this._viewModel.SavedContents.Add(data);
                }
            }
        }

        /// <summary>
        /// Delete all saved data in the sqlite database table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            this._connection.DeleteAll<MyData>();
            this._viewModel.SavedContents.Clear();
        }
    }
}
