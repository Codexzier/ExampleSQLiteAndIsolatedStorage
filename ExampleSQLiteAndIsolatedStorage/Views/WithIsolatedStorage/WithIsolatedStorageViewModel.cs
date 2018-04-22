using System.Collections.ObjectModel;

namespace ExampleSQLiteAndIsolatedStorage.Views.WithIsolatedStorage
{
    public class WithIsolatedStorageViewModel : BaseViewModel
    {
        private string _contentToSave;
        public string ContentToSave
        {
            get => this._contentToSave;
            set
            {
                this._contentToSave = value;
                this.OnPropertyChanged(nameof(this.ContentToSave));
            }
        }

        private ObservableCollection<MyData> _savedContents = new ObservableCollection<MyData>();
        public ObservableCollection<MyData> SavedContents
        {
            get => this._savedContents;
            set
            {
                this._savedContents = value;
                this.OnPropertyChanged(nameof(this.SavedContents));
            }
        }
    }
}
