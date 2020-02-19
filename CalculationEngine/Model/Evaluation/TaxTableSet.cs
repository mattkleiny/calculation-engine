using System;

namespace CalculationEngine.Model.Evaluation
{
  public sealed class TaxTableSet
  {
    public TaxTable PAYG { get; } = new TaxTable();
    public TaxTable HELP { get; } = new TaxTable();
    public TaxTable STLS { get; } = new TaxTable();

    public TaxTable this[TaxCategory category] => category switch
    {
      TaxCategory.PAYG => PAYG,
      TaxCategory.HELP => HELP,
      TaxCategory.STLS => STLS,
      _ => throw new ArgumentOutOfRangeException(nameof(category))
    };
  }
}