using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Tree;
using Xunit;

namespace CalculationEngine.Tests.Model
{
  public class CalculationTests
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
      Assert.True(explanation.Count > 0);
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
      Assert.True(output.Contains("PAYG"));
    }

    [Fact]
    public void it_should_parse_a_simple_linq_expression()
    {
      var graph  = Calculation.Parse(() => (100m + Amount * 100m) / 2m);
      var output = graph.Evaluate(new EvaluationContext());

      Assert.NotNull(graph);
      Assert.Equal(150m, output);
    }

    private static Calculation BuildSimpleGraph() => new Calculation(
      new BlockExpression(
        new AssignmentExpression(
          symbol: "A",
          operand: new TaxExpression(
            mode: TaxOperation.Add,
            category: TaxCategory.PAYG,
            value: new RoundingExpression(
              method: MidpointRounding.AwayFromZero,
              value: new SigmaExpression(
                new TallyExpression(EarningsCategory.Earnings),
                new TallyExpression(EarningsCategory.Allowances),
                new TallyExpression(EarningsCategory.Deductions),
                new TallyExpression(EarningsCategory.Leave)
              ),
              label: "Round to nearest dollar"
            ),
            label: "Apply PAYG amounts"
          )
        ),
        new AssignmentExpression(
          symbol: "B",
          new TaxExpression(
            mode: TaxOperation.Calculate,
            category: TaxCategory.HELP,
            value: new ConstantExpression(10_000m)
          )
        ),
        new SigmaExpression(
          new VariableExpression("A"),
          new VariableExpression("B")
        )
      )
    );
  }
}