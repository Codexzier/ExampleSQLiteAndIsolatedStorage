using SQLite.Net.Attributes;

namespace ExampleSQLiteAndIsolatedStorage
{
    public class MyData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Content { get; set; }

        public override string ToString() => $"{this.Id};{this.Content}";
    }
}
