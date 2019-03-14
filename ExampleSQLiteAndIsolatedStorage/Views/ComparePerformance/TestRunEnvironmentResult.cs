using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    public class TestRunEnvironmentResult
    {
        internal TestResults ResultSQLiteTests { get; set; }
        internal TestResults ResultsIsolatedStoargeTests { get; set; }
    }
}
