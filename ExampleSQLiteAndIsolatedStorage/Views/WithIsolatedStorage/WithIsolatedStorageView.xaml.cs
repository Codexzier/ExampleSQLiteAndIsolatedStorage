using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExampleSQLiteAndIsolatedStorage.Views.WithIsolatedStorage
{
    public sealed partial class WithIsolatedStorageView : UserControl
    {
        private WithIsolatedStorageViewModel _viewModel = new WithIsolatedStorageViewModel();
        private IsolatedStorageManager _isoStorage = new IsolatedStorageManager();

        public WithIsolatedStorageView()
        {
            this.InitializeComponent();

            this.DataContext = this._viewModel;
        }

        /// <summary>
        /// Load initial data or create file to add data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserControl_Loaded(object sender, RoutedEventArgs e) => await this.LoadData();

        /// <summary>
        /// Execute by clicking the button 'save' to store the text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await this._isoStorage.Save(new MyData { Content = this._viewModel.ContentToSave });
            await this.LoadData();
        }

        /// <summary>
        /// Load all saved data from isolated storage.
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            var list = await this._isoStorage.Load();
            foreach (var item in list)
            {
                if (!this._viewModel.SavedContents.Any(a => a.Id == item.Id))
                {
                    this._viewModel.SavedContents.Add(item);
                }
            }
        }

        /// <summary>
        /// Clear all the saved data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            await this._isoStorage.Clear();
            this._viewModel.SavedContents.Clear();
        }
    }
}
