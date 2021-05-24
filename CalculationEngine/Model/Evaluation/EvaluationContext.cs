using System;
using System.Collections.Generic;
using CalculationEngine.Model.Utilities;

namespace CalculationEngine.Model.Evaluation
{
  [Flags]
  public enum EarningsCategory
  {
    None             = 0,
    OrdinaryEarnings = 1 << 0,
    Allowances       = 1 << 1,
    Deductions       = 1 << 2,
    Leave            = 1 << 3,
    All              = OrdinaryEarnings | Allowances | Deductions | Leave
  }

  public enum TaxCategory
  {
    PAYG,
    SFSS,
    HELP,
    STSL,
  }

  public enum AnnualisationMethod
  {
    Weekly,
    Monthly,
    Yearly,
  }

  /// <summary>
  /// Defines the content for evaluation of <see cref="Calculation"/>s.
  /// <para/>
  /// Contextual data here includes access to the database and mechanisms with which
  /// to fetch tax coefficients, etc.
  /// </summary>
  public sealed class EvaluationContext
  {
    public EarningsSet Earnings  { get; } = new();
    public TaxTableSet TaxTables { get; } = new();
    public ResultSet   Results   { get; } = new();

    /// <summary>An primitive example of how to compose database access for easy consumption by the calculation.</summary>
    /// <remarks>This is just an example, and one could imagine a simple cache here to minimize database access.</remarks>
    public sealed class EarningsSet
    {
      public AnnualisedAmount CalculateEarnings(EarningsCategory category, AnnualisationMethod method)
      {
        var total = 0m;

        foreach (var value in category.GetMaskValues())
        {
          total += value switch
          {
            EarningsCategory.OrdinaryEarnings => 10_000m,
            EarningsCategory.Allowances => 5000m,
            EarningsCategory.Deductions => 0m,
            EarningsCategory.Leave => 0m,
            _ => 0m
          };
        }

        var periods = method switch
        {
          AnnualisationMethod.Weekly => 52,
          AnnualisationMethod.Monthly => 12,
          AnnualisationMethod.Yearly => 1,
          _ => throw new ArgumentOutOfRangeException(nameof(method))
        };

        return new AnnualisedAmount(total, periods);
      }
    }

    /// <summary>An primitive example of how to compose tax tables for easy consumption by the calculation.</summary>
    /// <remarks>This is just an example.</remarks>
    public sealed class TaxTableSet
    {
      public TaxTable PAYG { get; } = new();
      public TaxTable SFSS { get; } = new();
      public TaxTable HELP { get; } = new();
      public TaxTable STSL { get; } = new();

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
        /// <summary>Fetches coefficients for the given cumulative yearly income.</summary>
        public (decimal A, decimal B) this[decimal cumulative]
        {
          get => (0.20m, 200m); // chosen by fair and unbiased dice roll
        }
      }
    }

    /// <summary>A cache for intermediate results in a calculation.</summary>
    public sealed class ResultSet
    {
      private readonly Dictionary<string, decimal> results = new();

      public decimal GetOrCompute(string key, Func<decimal> factory)
      {
        if (!results.TryGetValue(key, out var result))
        {
          results[key] = result = factory();
        }

        return result;
      }
    }
  }
}