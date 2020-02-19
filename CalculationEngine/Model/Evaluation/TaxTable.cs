namespace CalculationEngine.Model.Evaluation
{
  public sealed class TaxTable
  {
    public decimal this[decimal cumulative]
    {
      get => 0.20m; // chosen by fair and unbiased dice roll
    }
  }
}