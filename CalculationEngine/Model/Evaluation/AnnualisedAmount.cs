using System;
using System.Runtime.CompilerServices;

namespace CalculationEngine.Model.Evaluation
{
  /// <summary>Represents and defers the evaluation of an annualised division.</summary>
  public readonly struct AnnualisedAmount : IEquatable<AnnualisedAmount>
  {
    public decimal Total   { get; }
    public decimal Periods { get; }
    public decimal Value   => Total / Periods;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AnnualisedAmount FromAnnualised(decimal amount, decimal periods)
    {
      return new(amount * periods, periods);
    }

    public AnnualisedAmount(decimal total, decimal periods)
    {
      Total   = total;
      Periods = periods;
    }

    public override string ToString() => $"{Total} over {Periods} periods ({Value})";

    public          bool Equals(AnnualisedAmount other) => Total == other.Total && Periods == other.Periods;
    public override bool Equals(object? obj)            => obj is AnnualisedAmount other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Total, Periods);

    public static bool operator ==(AnnualisedAmount left, AnnualisedAmount right) => left.Equals(right);
    public static bool operator !=(AnnualisedAmount left, AnnualisedAmount right) => !left.Equals(right);

    public static implicit operator decimal(AnnualisedAmount rate) => rate.Value;
  }
}