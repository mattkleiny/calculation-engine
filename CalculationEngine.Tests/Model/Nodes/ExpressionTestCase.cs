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

    internal abstract CalculationExpression Build();

    [Fact]
    public void it_should_evaluate()
    {
      var expression = new Calculation(Build());
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
      var expression = new Calculation(Build());
      var context    = new EvaluationContext();

      var explanation = expression.Explain(context);

      Assert.Equal(ExpectedSteps, explanation.Count);
    }

    [Fact]
    public void it_should_compile()
    {
      var expression = new Calculation(Build());

      var compiled = expression.Compile();

      Assert.NotNull(compiled);
    }

    [Fact]
    public void it_should_pretty_print()
    {
      var expression = Build();

      Assert.NotNull(expression.ToString());
    }
  }
}