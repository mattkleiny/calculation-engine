namespace CalculationEngine.Model
{
  public abstract class CalculationExpression
  {
    private CalculationExpression()
    {
    }

    public abstract T Accept<T>(Visitor<T> visitor);

    public enum UnaryOperator
    {
      Negate
    }

    public enum BinaryOperator
    {
      Plus,
      Minus,
      Times,
      Divide
    }

    public abstract class Visitor<T>
    {
      public virtual T Visit(Constant expression)
      {
        return default;
      }

      public virtual T Visit(Unary expression)
      {
        expression.Expression.Accept(this);

        return default;
      }

      public virtual T Visit(Binary expression)
      {
        expression.Left.Accept(this);
        expression.Right.Accept(this);

        return default;
      }

      public virtual T Visit(Grouping expression)
      {
        expression.Expression.Accept(this);

        return default;
      }

      public virtual T Visit(Round expression)
      {
        expression.Expression.Accept(this);

        return default;
      }

      public virtual T Visit(TaxTableLookup expression)
      {
        return default;
      }
    }

    public sealed class Constant : CalculationExpression
    {
      public decimal Value       { get; }
      public string  Label       { get; }
      public string  Description { get; }

      public Constant(decimal value, string label = null, string description = null)
      {
        Value       = value;
        Label       = label ?? string.Empty;
        Description = description ?? string.Empty;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class Unary : CalculationExpression
    {
      public UnaryOperator         Operator    { get; }
      public CalculationExpression Expression  { get; }
      public string                Label       { get; }
      public string                Description { get; }

      public Unary(UnaryOperator @operator, CalculationExpression expression, string label = null, string description = null)
      {
        Operator    = @operator;
        Expression  = expression;
        Label       = label ?? string.Empty;
        Description = description ?? string.Empty;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class Binary : CalculationExpression
    {
      public BinaryOperator        Operator    { get; }
      public CalculationExpression Left        { get; }
      public CalculationExpression Right       { get; }
      public string                Label       { get; }
      public string                Description { get; }

      public Binary(BinaryOperator @operator, CalculationExpression left, CalculationExpression right, string label = null, string description = null)
      {
        Operator    = @operator;
        Left        = left;
        Right       = right;
        Label       = label ?? string.Empty;
        Description = description ?? string.Empty;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class Grouping : CalculationExpression
    {
      public CalculationExpression Expression { get; }

      public Grouping(CalculationExpression expression)
      {
        Expression = expression;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class Round : CalculationExpression
    {
      public RoundingMethod        Method      { get; }
      public CalculationExpression Expression  { get; }
      public string                Label       { get; }
      public string                Description { get; }

      public Round(RoundingMethod method, CalculationExpression expression, string label = null, string description = null)
      {
        Method      = method;
        Expression  = expression;
        Label       = label ?? string.Empty;
        Description = description ?? string.Empty;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }

    public sealed class TaxTableLookup : CalculationExpression
    {
      public string Type        { get; }
      public string Label       { get; }
      public string Description { get; }

      public TaxTableLookup(string type, string label = null, string description = null)
      {
        Type        = type;
        Label       = label ?? string.Empty;
        Description = description ?? string.Empty;
      }

      public override T Accept<T>(Visitor<T> visitor) => visitor.Visit(this);
    }
  }
}