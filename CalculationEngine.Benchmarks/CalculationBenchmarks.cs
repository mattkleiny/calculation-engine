using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Benchmarks
{
  [SimpleJob(RuntimeMoniker.NetCoreApp31)]
  [MemoryDiagnoser]
  [Orderer(SummaryOrderPolicy.FastestToSlowest)]
  [RankColumn(NumeralSystem.Roman)]
  public class CalculationBenchmarks
  {
    public static void Main()
    {
      BenchmarkRunner.Run<CalculationBenchmarks>();
    }

    [Benchmark(Baseline = true)]
    public void InterpretCalculation()
    {
      ExampleProgram.Calculation.Interpet(new EvaluationContext());
    }

    [Benchmark]
    public void ExecuteCalculation()
    {
      ExampleProgram.Calculation.Execute(new EvaluationContext());
    }
  }
}