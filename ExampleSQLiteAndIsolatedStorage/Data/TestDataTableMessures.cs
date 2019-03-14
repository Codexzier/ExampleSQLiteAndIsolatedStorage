using SQLite.Net.Attributes;

namespace ExampleSQLiteAndIsolatedStorage.Data
{
    public class TestDataTableMessures
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public int Temperatur { get; set; }
        public double Pressure { get; set; }
        public float Humidity { get; set; }



        public long Time { get; set; }
        public long Room { get; set; }
    }
}
