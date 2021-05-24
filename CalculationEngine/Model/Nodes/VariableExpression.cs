using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Visitors;

namespace CalculationEngine.Model.Nodes
{
  internal sealed record VariableExpression(Symbol Symbol, CalculationExpression Operand, bool IncludeLabel) : CalculationExpression
  {
    internal override decimal Evaluate(EvaluationContext context)
    {
      return context.Results.GetOrCompute(Symbol.ToString(), () => Operand.Evaluate(context));
    }

    internal override void Explain(ExplanationContext context)
    {
      Operand.Explain(context);

      if (IncludeLabel)
      {
        context.AddStep(Symbol.ToString(), this);
      }
    }

    internal override T Accept<T>(ICalculationVisitor<T> visitor)
    {
      return visitor.Visit(this);
    }

    public override string ToString()
    {
      return $"({Symbol} = {Operand})";
    }
  }
}