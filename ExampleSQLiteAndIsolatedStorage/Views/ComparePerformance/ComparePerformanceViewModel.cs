using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    public class ComparePerformanceViewModel : BaseViewModel
    {
        private ObservableCollection<ListViewItemResult> _results = new ObservableCollection<ListViewItemResult>();
        public ObservableCollection<ListViewItemResult> Results
        {
            get => this._results;
            set
            {
                this._results = value;
                this.OnPropertyChanged(nameof(this.Results));
            }
        }

        private int _writeDataCount;
        public int WriteDataCount
        {
            get => this._writeDataCount;
            set
            {
                this._writeDataCount = value;
                this.OnPropertyChanged(nameof(this.WriteDataCount));
            }
        }

        private string _status;
        public string Status
        {
            get => this._status;
            set
            {
                this._status = value;
                this.OnPropertyChanged(nameof(this.Status));
            }
        }
    }
}
