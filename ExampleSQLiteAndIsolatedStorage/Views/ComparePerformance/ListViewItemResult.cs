using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    public class ListViewItemResult
    {
        public string LeftRowTitle { get; set; }

        public string SQLiteResultWrite { get; set; }
        public string SQLiteResultRead { get; set; }
        public string SQLiteResultReadWhere { get; set; }

        public string IsolatedStorageWrite { get; set; }
        public string IsolatedStorageRead { get; set; }
        public string IsolatedStorageReadWhere { get; set; }
    }
}
