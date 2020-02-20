using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;

namespace CalculationEngine.Model
{
  /// <summary>Permits static construction of <see cref="Calculation"/></summary>
  public sealed class CompiledCalculation
  {
    public static CompiledCalculation Create(Func<Calculation> factory) => new CompiledCalculation(factory());

    private readonly Calculation                      calculation;
    private readonly Func<EvaluationContext, decimal> compilation;

    private CompiledCalculation(Calculation calculation)
    {
      this.calculation = calculation;

      compilation = calculation.Compile();
    }

    public decimal Execute(EvaluationContext context)  => compilation(context);
    public decimal Interpet(EvaluationContext context) => calculation.Evaluate(context);

    public CalculationExplanation Explain(EvaluationContext context) => calculation.Explain(context);

    public override string ToString() => calculation.ToString();
  }
}