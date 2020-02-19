namespace CalculationEngine.Model.AST
{
  public abstract class CalculationExpression
  {
    public string Label { get; protected set; }

    public abstract decimal Evaluate();

    public abstract T Accept<T>(CalculationVisitor<T> visitor);
  }
}