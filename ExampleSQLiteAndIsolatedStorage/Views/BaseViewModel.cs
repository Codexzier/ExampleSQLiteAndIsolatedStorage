using System.ComponentModel;

namespace ExampleSQLiteAndIsolatedStorage.Views
{
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyname) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
