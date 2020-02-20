using System;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Model
{
  /// <summary>Permits static construction and compilation of <see cref="Calculation"/>s.</summary>
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

    public decimal                Execute(EvaluationContext context)  => compilation(context);
    public decimal                Interpet(EvaluationContext context) => calculation.Evaluate(context);
    public CalculationExplanation Explain(EvaluationContext context)  => calculation.Explain(context);

    internal T Accept<T>(ICalculationVisitor<T> visitor) => calculation.Accept(visitor);

    public override string ToString() => calculation.ToString();
  }
}