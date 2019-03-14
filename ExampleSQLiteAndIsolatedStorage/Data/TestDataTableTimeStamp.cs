using SQLite.Net.Attributes;
using System;

namespace ExampleSQLiteAndIsolatedStorage.Data
{
    public class TestDataTableTimeStamp
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public DateTime Time { get; set; }
    }
}
