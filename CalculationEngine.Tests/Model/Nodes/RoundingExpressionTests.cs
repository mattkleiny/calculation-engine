using System;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class RoundingExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 3m;

    internal override CalculationExpression BuildCalculation() => new RoundingExpression(
      Value: new ConstantExpression(2.49m),
      Rounding: MidpointRounding.ToPositiveInfinity
    );
  }
}