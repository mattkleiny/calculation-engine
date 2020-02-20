using System;
using CalculationEngine.Model.Utilities;

namespace CalculationEngine.Model.Evaluation
{
  [Flags]
  public enum EarningsCategory
  {
    None       = 0,
    Earnings   = 1 << 0,
    Allowances = 1 << 1,
    Deductions = 1 << 2,
    Leave      = 1 << 3,
    All        = Earnings | Allowances | Deductions | Leave
  }

  public enum TaxCategory
  {
    PAYG,
    SFSS,
    HELP,
    STSL,
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
      public decimal SumYearToDates(EarningsCategory category)
      {
        var total = 0m;

        foreach (var value in category.GetMaskValues())
        {
          total += value switch
          {
            EarningsCategory.Earnings => 10_000m,
            EarningsCategory.Allowances => 5000m,
            EarningsCategory.Deductions => 0m,
            EarningsCategory.Leave => 0m,
            _ => 0m
          };
        }

        return total;
      }
    }

    /// <summary>An primitive example of how to compose tax tables for easy consumption by the calculation.</summary>
    /// <remarks>This is just an example.</remarks>
    public sealed class TaxTableSet
    {
      public TaxTable PAYG { get; } = new TaxTable();
      public TaxTable SFSS { get; } = new TaxTable();
      public TaxTable HELP { get; } = new TaxTable();
      public TaxTable STSL { get; } = new TaxTable();

      public TaxTable this[TaxCategory category] => category switch
      {
        TaxCategory.PAYG => PAYG,
        TaxCategory.SFSS => SFSS,
        TaxCategory.HELP => HELP,
        TaxCategory.STSL => STSL,
        _ => throw new ArgumentOutOfRangeException(nameof(category))
      };

      public sealed class TaxTable
      {
        public (decimal A, decimal B) this[decimal cumulative]
        {
          get => (0.20m, 200m); // chosen by fair and unbiased dice roll
        }
      }
    }
  }
}