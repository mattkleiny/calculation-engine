namespace CalculationEngine.Model.AST
{
  public abstract class CalculationVisitor<T>
  {
    public virtual T Visit(ConstantExpression expression)
    {
      return default;
    }

    public virtual T Visit(UnaryExpression expression)
    {
      expression.Operand.Accept(this);

      return default;
    }

    public virtual T Visit(BinaryExpression expression)
    {
      expression.Left.Accept(this);
      expression.Right.Accept(this);

      return default;
    }

    public virtual T Visit(RoundingExpression expression)
    {
      expression.Value.Accept(this);

      return default;
    }
  }
}