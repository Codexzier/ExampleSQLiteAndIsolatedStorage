using SQLite.Net.Attributes;
using System;

namespace ExampleSQLiteAndIsolatedStorage.Data
{
    public class TestData
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public int Temperatur { get; set; }
        public double Pressure { get; set; }
        public float Humidity { get; set; }
        public DateTime Time { get; set; }
        public string Room { get; set; }
    }
}
