namespace CalculationEngine.Model.Evaluation
{
  public sealed class EvaluationContext
  {
    public VariableSet Variables { get; } = new VariableSet();
    public TaxTableSet TaxTables { get; } = new TaxTableSet();
  }
}