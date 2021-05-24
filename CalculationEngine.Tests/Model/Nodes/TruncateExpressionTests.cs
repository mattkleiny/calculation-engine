using System;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class TruncateExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 3m;

    internal override CalculationExpression BuildCalculation() => new TruncateExpression(
      Value: new ConstantExpression((decimal) Math.PI)
    );
  }
}