using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class TallyExpressionTests : ExpressionTestCase
  {
    internal override CalculationExpression Build() => new TallyExpression(EarningsCategory.All);
  }
}