using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using static CalculationEngine.Model.CalculationExpression;

namespace CalculationEngine.Model
{
  // TODO: allow parsing graphs from text?
  // TODO: allow parsing graphs from C# expressions?
  // TODO: add a fluent 'builder' pattern on top of this

  public delegate decimal Calculation();

  public sealed class CalculationGraph
  {
    private readonly CalculationExpression expression;

    public CalculationGraph(CalculationExpression expression)
    {
      this.expression = expression;
    }

    public decimal Evaluate()
    {
      return expression.Accept(new EvaluationVisitor());
    }

    public Calculation ToDelegate()
    {
      var expression = ToLinqExpression();
      var invocation = Expression.Lambda(expression).Compile();

      return () => (decimal) invocation.DynamicInvoke();
    }

    public Expression ToLinqExpression()
    {
      return expression.Accept(new CompilationVisitor());
    }

    public CalculationExplanation ToExplanation()
    {
      return new CalculationExplanation(expression.Accept(new ExplanationVisitor()));
    }

    public override string ToString()
    {
      return expression.Accept(new PrettyPrintVisitor());
    }

    private sealed class PrettyPrintVisitor : Visitor<string>
    {
      public override string Visit(Constant expression)
      {
        return expression.Value.ToString("F");
      }

      public override string Visit(Unary expression)
      {
        var inner = expression.Expression.Accept(this);

        return $"{ConvertToString(expression.Operator)} {inner}";
      }

      public override string Visit(Binary expression)
      {
        var left  = expression.Left.Accept(this);
        var right = expression.Right.Accept(this);

        return $"{left} {ConvertToString(expression.Operator)} {right}";
      }

      public override string Visit(Grouping expression)
      {
        return $"({expression.Expression.Accept(this)})";
      }

      public override string Visit(Round expression)
      {
        return expression.Expression.Accept(this);
      }

      public override string Visit(TaxTableLookup expression)
      {
        return $"Tax Table ({expression.Type})";
      }

      private string ConvertToString(UnaryOperator @operator)
      {
        switch (@operator)
        {
          case UnaryOperator.Negate: return "-";

          default:
            throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
        }
      }

      private string ConvertToString(BinaryOperator @operator)
      {
        switch (@operator)
        {
          case BinaryOperator.Plus: return "+";
          case BinaryOperator.Minus: return "-";
          case BinaryOperator.Times: return "*";
          case BinaryOperator.Divide: return "/";

          default:
            throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null);
        }
      }
    }

    private sealed class EvaluationVisitor : Visitor<decimal>
    {
      public override decimal Visit(Constant expression)
      {
        return expression.Value;
      }

      public override decimal Visit(Unary expression)
      {
        switch (expression.Operator)
        {
          case UnaryOperator.Negate:
            return -expression.Expression.Accept(this);
        }

        return 0m;
      }

      public override decimal Visit(Binary expression)
      {
        var left  = expression.Left.Accept(this);
        var right = expression.Right.Accept(this);

        switch (expression.Operator)
        {
          case BinaryOperator.Plus: return left + right;
          case BinaryOperator.Minus: return left - right;
          case BinaryOperator.Times: return left * right;
          case BinaryOperator.Divide: return left / right;
        }

        return 0m;
      }

      public override decimal Visit(Grouping expression)
      {
        return expression.Expression.Accept(this);
      }

      public override decimal Visit(Round expression)
      {
        var amount = expression.Expression.Accept(this);

        return Math.Round(amount, MidpointRounding.AwayFromZero);
      }

      public override decimal Visit(TaxTableLookup expression)
      {
        throw new NotImplementedException();
      }
    }

    private sealed class ExplanationVisitor : Visitor<IEnumerable<CalculationExplanation.Step>>
    {
      public override IEnumerable<CalculationExplanation.Step> Visit(Constant expression)
      {
        throw new NotImplementedException();
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(Unary expression)
      {
        throw new NotImplementedException();
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(Binary expression)
      {
        throw new NotImplementedException();
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(Grouping expression)
      {
        throw new NotImplementedException();
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(Round expression)
      {
        throw new NotImplementedException();
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(TaxTableLookup expression)
      {
        throw new NotImplementedException();
      }
    }

    private sealed class CompilationVisitor : Visitor<Expression>
    {
      public override Expression Visit(Constant expression)
      {
        return Expression.Constant(expression.Value, typeof(decimal));
      }

      public override Expression Visit(Unary expression)
      {
        switch (expression.Operator)
        {
          case UnaryOperator.Negate:
            return Expression.Not(expression.Expression.Accept(this));

          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      public override Expression Visit(Binary expression)
      {
        switch (expression.Operator)
        {
          case BinaryOperator.Plus:
            return Expression.AddChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperator.Minus:
            return Expression.SubtractChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperator.Times:
            return Expression.MultiplyChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperator.Divide:
            return Expression.Divide(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      public override Expression Visit(Grouping expression)
      {
        return expression.Expression.Accept(this);
      }

      public override Expression Visit(Round expression)
      {
        var method = typeof(Math).GetMethod(nameof(Math.Round), new[] { typeof(decimal), typeof(MidpointRounding) });

        if (method == null)
        {
          throw new Exception($"Unable to locate {nameof(Math)}.{nameof(Round)} method; has the version of the .NET framework changed?");
        }

        var        argument1 = expression.Expression.Accept(this);
        Expression argument2 = Expression.Constant(MidpointRounding.AwayFromZero);

        return Expression.Call(method, argument1, argument2);
      }

      public override Expression Visit(TaxTableLookup expression)
      {
        throw new NotImplementedException();
      }
    }
  }
}