using System;
using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Nodes;
using Xunit;
using static CalculationEngine.Model.Calculation;
using static CalculationEngine.Model.Evaluation.EarningsCategory;
using static CalculationEngine.Model.Evaluation.TaxCategory;

namespace CalculationEngine.Tests.Model
{
  public class CalculationTests
  {
    [Fact]
    public void it_should_evaluate_a_simple_graph()
    {
      var calculation = BuildSimpleCalculation();
      var output      = calculation.Evaluate(new EvaluationContext());

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_compile_to_an_explanation()
    {
      var graph       = BuildSimpleCalculation();
      var explanation = graph.Explain(new EvaluationContext());

      Assert.NotNull(explanation);
      Assert.True(explanation.Count > 0);
    }

    [Fact]
    public void it_should_compile_to_a_delegate()
    {
      var graph       = BuildSimpleCalculation();
      var calculation = graph.Compile();

      Assert.NotNull(calculation);

      var output = calculation(new EvaluationContext());

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_pretty_print_to_a_string()
    {
      var graph  = BuildSimpleCalculation();
      var output = graph.ToString();

      Assert.NotNull(output);
      Assert.True(output.Contains("PAYG"));
    }

    [Fact]
    public void it_should_build_a_simple_calculation_fluently()
    {
      var calculation = Round(YTD(Earnings) + YTD(Allowances) + YTD(Deductions) + YTD(Leave));
      var output      = calculation.Evaluate(new EvaluationContext());

      Assert.True(calculation.ToString().Contains("Earnings"));
      Assert.True(output > 0m);
    }

    private static Calculation BuildSimpleCalculation()
    {
      return new SigmaExpression(
        new ConstantExpression(10_000m),
        new TaxExpression(category: PAYG,
          value: new RoundingExpression(value: new SigmaExpression(
              new TallyExpression(Earnings),
              new TallyExpression(Allowances),
              new TallyExpression(Deductions),
              new TallyExpression(Leave)
            ),
            method: MidpointRounding.AwayFromZero)
        )
      );
    }
  }
}