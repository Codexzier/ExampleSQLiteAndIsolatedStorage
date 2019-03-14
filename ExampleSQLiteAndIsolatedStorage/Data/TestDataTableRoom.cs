using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Data
{
    public class TestDataTableRoom
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Room { get; set; }
    }
}
