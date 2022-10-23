using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firely.Sdk.Benchmarks
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestPocoDictionary()
        {
            var sut = new FhirPathBenchmarks();
            sut.BenchmarkSetup();
            sut.DictionaryElementNode();
        }
    }
}
