using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;
using Xunit;

namespace CalculationEngine.Tests.Model.Nodes
{
  public abstract class ExpressionTestCase
  {
    protected virtual int      ExpectedSteps  { get; } = 0;
    protected virtual decimal? ExpectedOutput { get; } = null;

    internal abstract CalculationExpression BuildCalculation();

    [Fact]
    public void it_should_evaluate()
    {
      var expression = new Calculation(BuildCalculation());
      var context    = new EvaluationContext();

      var output = expression.Evaluate(context);

      if (ExpectedOutput.HasValue)
      {
        Assert.Equal(ExpectedOutput, output);
      }
    }

    [Fact]
    public void it_should_explain()
    {
      var expression = new Calculation(BuildCalculation());
      var context    = new EvaluationContext();

      var explanation = expression.Explain(context);

      Assert.Equal(ExpectedSteps, explanation.Count);
    }

    [Fact]
    public void it_should_pretty_print()
    {
      var expression = BuildCalculation();

      Assert.NotNull(expression.ToString());
    }
  }
}