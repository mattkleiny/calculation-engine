using System.Collections.Generic;
using CalculationEngine.Model.Tree;

namespace CalculationEngine.Model.Evaluation
{
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