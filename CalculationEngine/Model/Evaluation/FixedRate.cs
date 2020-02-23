using System;
using System.Runtime.CompilerServices;

namespace CalculationEngine.Model.Evaluation
{
  /// <summary>Represents and defers the evaluation of a fixed rate calculation.</summary>
  public readonly struct FixedRate : IEquatable<FixedRate>
  {
    public decimal Rate   { get; }
    public decimal Amount { get; }
    public decimal Value  => Rate * Amount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FixedRate FromFixedRate(decimal total, decimal rate)
    {
      return new FixedRate(total / rate, rate);
    }

    public FixedRate(decimal amount, decimal rate)
    {
      Rate   = rate;
      Amount = amount;
    }

    public override string ToString() => $"{Rate} at {Amount} ({Value})";

    public          bool Equals(FixedRate other) => Rate == other.Rate && Amount == other.Amount;
    public override bool Equals(object? obj)     => obj is FixedRate other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Rate, Amount);

    public static bool operator ==(FixedRate left, FixedRate right) => left.Equals(right);
    public static bool operator !=(FixedRate left, FixedRate right) => !left.Equals(right);

    public static implicit operator decimal(FixedRate rate) => rate.Value;
  }
}