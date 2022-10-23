using BenchmarkDotNet.Running;

namespace Firely.Sdk.Benchmarks
{
    /// <summary>
    /// To run this, go to the .csproj directory and run
    /// <code>dotnet run -c Release --filter *FhirPath*</code>
    /// </summary>
    public class Program
    {
        //static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

        //static void Main(string[] args) => FhirPathBenchmarks.Debug();
    }
}
