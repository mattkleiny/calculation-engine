using System;
using CalculationEngine.Model;
using CalculationEngine.Model.AST;
using CalculationEngine.Model.Evaluation;
using Xunit;

namespace CalculationEngine.Tests.Model
{
  public class CalculationGraphTests
  {
    private static readonly decimal Amount = 2m;

    [Fact]
    public void it_should_evaluate_a_simple_graph()
    {
      var graph   = BuildSimpleGraph();
      var context = new EvaluationContext();
      var output  = graph.Evaluate(context);

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_compile_to_an_explanation()
    {
      var graph       = BuildSimpleGraph();
      var context     = new EvaluationContext();
      var explanation = graph.Explain(context);

      Assert.NotNull(explanation);
    }

    [Fact]
    public void it_should_compile_to_a_delegate()
    {
      var graph       = BuildSimpleGraph();
      var calculation = graph.Compile();

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

    [Fact]
    public void it_should_parse_a_simple_linq_expression()
    {
      var graph  = Calculation.Parse(() => (100m + Amount * 100m) / 2m);
      var output = graph.Evaluate(new EvaluationContext());

      Assert.NotNull(graph);
      Assert.Equal(150m, output);
    }

    private static Calculation BuildSimpleGraph()
    {
      return new Calculation(
        new ApplyTaxExpression(
          TaxCategory.PAYG,
          new RoundingExpression(
            MidpointRounding.AwayFromZero,
            new BinaryExpression(
              BinaryOperation.Divide,
              new BinaryExpression(
                BinaryOperation.Multiply,
                new ConstantExpression(10m),
                new ConstantExpression(20m),
                label: "Sum Year-to-dates"
              ),
              new ConstantExpression(2.5m)
            ),
            label: "Round to nearest dollar"
          ),
          label: "Apply PAYG amounts"
        )
      );
    }
  }
}