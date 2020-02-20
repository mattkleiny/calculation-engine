using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Benchmarks
{
  [SimpleJob(RuntimeMoniker.NetCoreApp31)]
  public class CalculationBenchmarks
  {
    public static void Main()
    {
      BenchmarkRunner.Run<CalculationBenchmarks>();
    }

    [Benchmark]
    public void ExecuteCalculation()
    {
      Example.Calculation.Execute(new EvaluationContext());
    }

    [Benchmark]
    public void InterpretCalculation()
    {
      Example.Calculation.Interpet(new EvaluationContext());
    }
  }
}