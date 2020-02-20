using CalculationEngine.Model;
using CalculationEngine.Model.Evaluation;
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
      var calculation = GetCalculation();
      var output      = calculation.Evaluate(new EvaluationContext());

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_compile_to_an_explanation()
    {
      var graph       = GetCalculation();
      var explanation = graph.Explain(new EvaluationContext());

      Assert.NotNull(explanation);
      Assert.True(explanation.Count > 0);
    }

    [Fact]
    public void it_should_compile_to_a_delegate()
    {
      var graph       = GetCalculation();
      var calculation = graph.Compile();

      Assert.NotNull(calculation);

      var output = calculation(new EvaluationContext());

      Assert.True(output > 0m);
    }

    [Fact]
    public void it_should_pretty_print_to_a_string()
    {
      var graph  = GetCalculation();
      var output = graph.ToString();

      Assert.NotNull(output);
      Assert.True(output.Contains("PAYG"));
    }

    private static Calculation GetCalculation()
    {
      var earnings = Variable("A", Sum(YTD(Earnings), YTD(Allowances), YTD(Deductions), YTD(Leave)));
      var tax      = Variable("B", Tax(PAYG, earnings));

      return Label("Σ", Round(earnings - tax));
    }
  }
}