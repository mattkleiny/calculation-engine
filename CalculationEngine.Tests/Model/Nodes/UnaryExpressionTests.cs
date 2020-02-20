using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class UnaryExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => -200m;

    internal override CalculationExpression BuildCalculation() => new UnaryExpression(
      operation: UnaryOperation.Negate,
      operand: new ConstantExpression(200m)
    );
  }
}