using System;

namespace CalculationEngine.Model.Evaluation
{
  public enum EarningsCategory
  {
    Earnings,
    Allowances,
    Deductions,
    Leave,
  }

  public enum TaxCategory
  {
    PAYG,
    HELP,
    STLS
  }

  /// <summary>
  /// Defines the content for evaluation of <see cref="Calculation"/>s.
  /// <para/>
  /// Contextual data here includes access to the database and mechanisms with which
  /// to fetch tax coefficients, etc.
  /// </summary>
  public sealed class EvaluationContext
  {
    public EarningsSet Earnings  { get; } = new EarningsSet();
    public TaxTableSet TaxTables { get; } = new TaxTableSet();

    /// <summary>An primitive example of how to compose database access for easy consumption by the calculation.</summary>
    /// <remarks>This is just an example, and one could imagine a simple cache here to minimize database access.</remarks>
    public sealed class EarningsSet
    {
      public decimal SumYearToDates(EarningsCategory category) => 10_000m;
    }

    /// <summary>An primitive example of how to compose tax tables for easy consumption by the calculation.</summary>
    /// <remarks>This is just an example.</remarks>
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

      public sealed class TaxTable
      {
        public decimal this[decimal cumulative]
        {
          get => 0.20m; // chosen by fair and unbiased dice roll
        }
      }
    }
  }
}