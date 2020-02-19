using System;
using System.Collections.Generic;
using CalculationEngine.Utilities;

namespace CalculationEngine.Model.Evaluation
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

  public sealed class EvaluationContext
  {
    public VariableSet Variables { get; } = new VariableSet();
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

    public sealed class VariableSet
    {
      private readonly Dictionary<string, decimal> symbols = new Dictionary<string, decimal>();

      public decimal this[Symbol symbol]
      {
        get => TryGet(symbol);
        set => symbols[symbol.Name] = value;
      }

      private decimal TryGet(Symbol symbol, decimal defaultValue = 0m)
      {
        if (symbols.TryGetValue(symbol.Name, out var value))
        {
          return value;
        }

        return defaultValue;
      }
    }
  }
}