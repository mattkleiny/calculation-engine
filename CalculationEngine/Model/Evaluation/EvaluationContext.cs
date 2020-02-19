using System;

namespace CalculationEngine.Model.Evaluation
{
  public enum TaxCategory
  {
    PAYG,
    HELP,
    STLS
  }

  public sealed class EvaluationContext
  {
    public TaxTableSet TaxTables { get; } = new TaxTableSet();

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

    public sealed class TaxTable
    {
      public decimal this[decimal cumulative]
      {
        get => 0.20m; // chosen by fair and unbiased dice roll
      }
    }
  }
}