using CalculationEngine.Model;
using Xunit;
using static CalculationEngine.Model.CalculationExpression;

namespace CalculationEngine.Tests.Model
{
  public class CalculationGraphTests
  {
    [Fact]
    public void it_should_evaluate_a_simple_graph()
    {
      var graph  = BuildSimpleGraph();
      var output = graph.Evaluate();

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_compile_to_an_explanation()
    {
      var graph       = BuildSimpleGraph();
      var explanation = graph.ToExplanation();

      Assert.NotNull(explanation);
    }

    [Fact]
    public void it_should_compile_to_a_LINQ_expression()
    {
      var graph      = BuildSimpleGraph();
      var expression = graph.ToLinqExpression();

      Assert.NotNull(expression);
    }

    [Fact]
    public void it_should_compile_to_a_delegate()
    {
      var graph       = BuildSimpleGraph();
      var calculation = graph.ToDelegate();

      Assert.NotNull(calculation);

      var output = calculation();

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_pretty_print_to_a_string()
    {
      var graph  = BuildSimpleGraph();
      var output = graph.ToString();

      Assert.NotNull(output);
      Assert.True(output.Contains("2.5"));
    }

    private static CalculationGraph BuildSimpleGraph()
    {
      return new CalculationGraph(new Round(
        RoundingMethod.AwayFromZero,
        new Grouping(
          new Binary(
            BinaryOperator.Divide,
            new Grouping(
              new Binary(
                BinaryOperator.Times,
                new Constant(10m),
                new Constant(20m))),
            new Constant(2.5m)))));
    }
  }
}