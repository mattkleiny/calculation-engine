using System;
using CalculationEngine.Model.Nodes;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class RoundingExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 3m;

    internal override CalculationExpression Build() => new RoundingExpression(
      value: new ConstantExpression(2.49m),
      method: MidpointRounding.ToPositiveInfinity
    );
  }
}