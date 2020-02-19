using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine
{
  public static class Program
  {
    public static void Main()
    {
      var graph       = Calculation.Parse(() => (100m + 200m) / 3m);
      var context     = new EvaluationContext();
      var explanation = graph.Explain(context);

      Console.WriteLine("Steps: ");
      foreach (var step in explanation)
      {
        Console.WriteLine(step.ToString());
      }

      Console.Write("Calculation: ");
      Console.WriteLine(graph.ToString());

      Console.Write("Output: ");
      Console.WriteLine(graph.Evaluate(context));
    }
  }
}