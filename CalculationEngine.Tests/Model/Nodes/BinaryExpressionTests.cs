using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;
using Xunit;

namespace CalculationEngine.Tests.Model.Nodes
{
  public sealed class BinaryExpressionTests : ExpressionTestCase
  {
    protected override decimal? ExpectedOutput => 12.5m;

    internal override CalculationExpression BuildCalculation() => new BinaryExpression(
      Operation: BinaryOperation.Add,
      Left: new ConstantExpression(10m),
      Right: new ConstantExpression(2.5m)
    );

    [Fact]
    public void it_should_add_when_evaluating()
    {
      var expression = new BinaryExpression(
        Operation: BinaryOperation.Add,
        Left: new ConstantExpression(10m),
        Right: new ConstantExpression(2.5m)
      );

      var output = expression.Evaluate(new EvaluationContext());

      Assert.Equal(12.5m, output);
    }

    [Fact]
    public void it_should_subtract_when_evaluating()
    {
      var expression = new BinaryExpression(
        Operation: BinaryOperation.Subtract,
        Left: new ConstantExpression(10m),
        Right: new ConstantExpression(2.5m)
      );

      var output = expression.Evaluate(new EvaluationContext());

      Assert.Equal(7.5m, output);
    }

    [Fact]
    public void it_should_multiply_when_evaluating()
    {
      var expression = new BinaryExpression(
        Operation: BinaryOperation.Multiply,
        Left: new ConstantExpression(10m),
        Right: new ConstantExpression(2.5m)
      );

      var output = expression.Evaluate(new EvaluationContext());

      Assert.Equal(25m, output);
    }

    [Fact]
    public void it_should_divide_when_evaluating()
    {
      var expression = new BinaryExpression(
        Operation: BinaryOperation.Divide,
        Left: new ConstantExpression(10m),
        Right: new ConstantExpression(2.5m)
      );

      var output = expression.Evaluate(new EvaluationContext());

      Assert.Equal(4m, output);
    }
  }
}