using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleSQLiteAndIsolatedStorage.Views.ComparePerformance
{
    internal class TestResults
    {
        internal TestResults(List<Tuple<string, string>> resultSaved, List<Tuple<string, string>> resultsRead, List<Tuple<string, string>> resultsReadWhere)
        {
            this.ResultsSaved = resultSaved;
            this.ResultsRead = resultsRead;
            this.ResultsReadWhere = resultsReadWhere;
        }

        internal List<Tuple<string, string>> ResultsSaved { get;  }
        internal List<Tuple<string, string>> ResultsRead { get;  }
        internal List<Tuple<string, string>> ResultsReadWhere { get; }
    }
}
