using System;

namespace CalculationEngine.Model.Evaluation
{
  public sealed class TaxTableSet
  {
    public EvaluationContext.TaxTable PAYG { get; } = new EvaluationContext.TaxTable();
    public EvaluationContext.TaxTable HELP { get; } = new EvaluationContext.TaxTable();
    public EvaluationContext.TaxTable STLS { get; } = new EvaluationContext.TaxTable();

    public EvaluationContext.TaxTable this[TaxCategory category] => category switch
    {
      TaxCategory.PAYG => PAYG,
      TaxCategory.HELP => HELP,
      TaxCategory.STLS => STLS,
      _ => throw new ArgumentOutOfRangeException(nameof(category))
    };
  }
}