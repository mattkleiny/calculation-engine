using System;

namespace CalculationEngine.Model.Evaluation
{
  /// <summary>A symbol name for use in variable bindings.</summary>
  internal readonly struct Symbol : IEquatable<Symbol>
  {
    private readonly string name;

    public Symbol(string name)
    {
      if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

      this.name = name;
    }

    public override string ToString() => name;

    public          bool Equals(Symbol other) => name == other.name;
    public override bool Equals(object? obj)  => obj is Symbol other && Equals(other);

    public override int GetHashCode() => name.GetHashCode();

    public static bool operator ==(Symbol left, Symbol right) => left.Equals(right);
    public static bool operator !=(Symbol left, Symbol right) => !left.Equals(right);

    public static implicit operator Symbol(string name) => new(name);
  }
}