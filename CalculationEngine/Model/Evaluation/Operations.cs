using System;

namespace CalculationEngine.Model.Evaluation
{
  public static class Operations
  {
    private static dynamic NoOp => throw new Exception("This class is used for LINQ binding, and cannot be invoked directly!");

    public static decimal Tax(TaxCategory category, decimal amount) => NoOp;
    public static decimal YTD(EarningsCategory category)            => NoOp;
  }
}