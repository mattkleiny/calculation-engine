namespace CalculationEngine.Model.Nodes
{
  internal interface ICalculationVisitor<out T>
  {
    T Visit(BinaryExpression expression);
    T Visit(ConstantExpression expression);
    T Visit(LabelExpression expression);
    T Visit(RoundingExpression expression);
    T Visit(SigmaExpression expression);
    T Visit(TallyExpression expression);
    T Visit(TruncateExpression expression);
    T Visit(TaxExpression expression);
    T Visit(UnaryExpression expression);
  }

  /// <summary>A <see cref="ICalculationVisitor{T}"/> which automatically walks the tree.</summary>
  internal abstract class CalculationVisitor<T> : ICalculationVisitor<T>
  {
    public virtual T Visit(BinaryExpression expression)
    {
      expression.Left.Accept(this);
      expression.Right.Accept(this);

      return default;
    }

    public virtual T Visit(ConstantExpression expression)
    {
      return default;
    }

    public virtual T Visit(LabelExpression expression)
    {
      expression.Expression.Accept(this);

      return default;
    }

    public virtual T Visit(RoundingExpression expression)
    {
      expression.Value.Accept(this);

      return default;
    }

    public virtual T Visit(SigmaExpression expression)
    {
      foreach (var subexpression in expression.Expressions)
      {
        subexpression.Accept(this);
      }

      return default;
    }

    public virtual T Visit(TallyExpression expression)
    {
      return default;
    }

    public virtual T Visit(TruncateExpression expression)
    {
      expression.Value.Accept(this);

      return default;
    }

    public virtual T Visit(TaxExpression expression)
    {
      expression.Value.Accept(this);

      return default;
    }

    public virtual T Visit(UnaryExpression expression)
    {
      expression.Operand.Accept(this);

      return default;
    }
  }
}