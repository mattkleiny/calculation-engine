using System;
using CalculationEngine.Utilities;

namespace CalculationEngine.Model.Tree
{
  public readonly struct Symbol : IEquatable<Symbol>
  {
    public string Name { get; }

    public Symbol(string name)
    {
      Check.NotNullOrEmpty(name, nameof(name));

      Name = name;
    }

    public override string ToString() => Name;

    public          bool Equals(Symbol other) => Name == other.Name;
    public override bool Equals(object? obj)  => obj is Symbol other && Equals(other);

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(Symbol left, Symbol right) => left.Equals(right);
    public static bool operator !=(Symbol left, Symbol right) => !left.Equals(right);

    public static implicit operator Symbol(string name) => new Symbol(name);
  }
}