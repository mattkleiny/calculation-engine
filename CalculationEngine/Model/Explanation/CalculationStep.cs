namespace CalculationEngine.Model.Explanation
{
  public sealed class CalculationStep
  {
    public string  Label       { get; }
    public string  Description { get; }
    public decimal Amount      { get; }

    public CalculationStep(string label, string description, decimal amount)
    {
      Label       = label;
      Description = description;
      Amount      = amount;
    }

    public override string ToString() => $"Step {Description} ({Label})";
  }
}