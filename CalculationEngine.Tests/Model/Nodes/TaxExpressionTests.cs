using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class TaxExpressionTests : ExpressionTestCase
  {
    internal override CalculationExpression BuildCalculation() => new TaxExpression(
      category: TaxCategory.HELP,
      value: new ConstantExpression(10_000m)
    );
  }
}