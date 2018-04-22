using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
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
