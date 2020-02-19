using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.AST;
using BinaryExpression = CalculationEngine.Model.AST.BinaryExpression;
using ConstantExpression = CalculationEngine.Model.AST.ConstantExpression;
using UnaryExpression = CalculationEngine.Model.AST.UnaryExpression;

namespace CalculationEngine.Model
{
  // TODO: allow parsing graphs from text?
  // TODO: add a fluent 'builder' pattern on top of this

  public delegate decimal Calculation();

  public sealed class CalculationGraph
  {
    private readonly CalculationExpression expression;

    public static CalculationGraph FromExpression(Expression<Calculation> expression)
    {
      var visitor = new TranslationVisitor();

      visitor.Visit(expression);

      return new CalculationGraph(visitor.Expressions.Dequeue());
    }

    public CalculationGraph(CalculationExpression expression)
    {
      this.expression = expression;
    }

    public decimal Evaluate() => expression.Evaluate();

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
      return expression.ToString();
    }

    private sealed class ExplanationVisitor : CalculationVisitor<IEnumerable<CalculationExplanation.Step>>
    {
      public override IEnumerable<CalculationExplanation.Step> Visit(ConstantExpression expression)
      {
        if (!string.IsNullOrEmpty(expression.Label))
        {
          yield return new CalculationExplanation.Step(expression);
        }
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(UnaryExpression expression)
      {
        foreach (var step in expression.Operand.Accept(this))
        {
          yield return step;
        }

        if (!string.IsNullOrEmpty(expression.Label))
        {
          yield return new CalculationExplanation.Step(expression);
        }
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(BinaryExpression expression)
      {
        foreach (var step in expression.Left.Accept(this))
        {
          yield return step;
        }

        foreach (var step in expression.Right.Accept(this))
        {
          yield return step;
        }

        if (!string.IsNullOrEmpty(expression.Label))
        {
          yield return new CalculationExplanation.Step(expression);
        }
      }

      public override IEnumerable<CalculationExplanation.Step> Visit(RoundingExpression expression)
      {
        foreach (var step in expression.Value.Accept(this))
        {
          yield return step;
        }

        if (!string.IsNullOrEmpty(expression.Label))
        {
          yield return new CalculationExplanation.Step(expression);
        }
      }
    }

    private sealed class CompilationVisitor : CalculationVisitor<Expression>
    {
      public override Expression Visit(ConstantExpression expression)
      {
        return Expression.Constant(expression.Value, typeof(decimal));
      }

      public override Expression Visit(UnaryExpression expression)
      {
        switch (expression.Operation)
        {
          case UnaryOperation.Not:
            return Expression.Not(expression.Operand.Accept(this));

          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      public override Expression Visit(BinaryExpression expression)
      {
        switch (expression.Operation)
        {
          case BinaryOperation.Add:
            return Expression.AddChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperation.Subtract:
            return Expression.SubtractChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperation.Multiply:
            return Expression.MultiplyChecked(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          case BinaryOperation.Divide:
            return Expression.Divide(
              expression.Left.Accept(this),
              expression.Right.Accept(this)
            );

          default:
            throw new ArgumentOutOfRangeException();
        }
      }

      public override Expression Visit(RoundingExpression expression)
      {
        var method = typeof(Math).GetMethod(nameof(Math.Round), new[] { typeof(decimal), typeof(MidpointRounding) });

        if (method == null)
        {
          throw new Exception($"Unable to locate {nameof(Math)}.{nameof(RoundingExpression)} method; has the version of the .NET framework changed?");
        }

        var argument1 = expression.Value.Accept(this);
        var argument2 = Expression.Constant(MidpointRounding.AwayFromZero);

        return Expression.Call(method, argument1, argument2);
      }
    }

    private sealed class TranslationVisitor : ExpressionVisitor
    {
      public Queue<CalculationExpression> Expressions { get; } = new Queue<CalculationExpression>();

      protected override Expression VisitConstant(System.Linq.Expressions.ConstantExpression node)
      {
        base.VisitConstant(node);

        Expressions.Enqueue(new ConstantExpression(Convert.ToDecimal(node.Value)));

        return node;
      }

      protected override Expression VisitUnary(System.Linq.Expressions.UnaryExpression node)
      {
        base.VisitUnary(node);

        var type = ConvertUnaryOperator(node.NodeType);

        Expressions.Enqueue(new UnaryExpression(type, Expressions.Dequeue()));

        return node;
      }

      protected override Expression VisitBinary(System.Linq.Expressions.BinaryExpression node)
      {
        base.VisitBinary(node);

        var type = ConvertBinaryOperator(node.NodeType);

        Expressions.Enqueue(new BinaryExpression(type, Expressions.Dequeue(), Expressions.Dequeue()));

        return node;
      }

      protected override Expression VisitMember(MemberExpression node)
      {
        base.VisitMember(node);

        var owner = ResolveOwner(node.Expression);

        switch (node.Member)
        {
          case FieldInfo field:
            Expressions.Enqueue(new ConstantExpression(Convert.ToDecimal(field.GetValue(owner))));
            break;

          case PropertyInfo property:
            Expressions.Enqueue(new ConstantExpression(Convert.ToDecimal(property.GetValue(owner))));
            break;

          case MethodInfo _:
            throw new NotSupportedException("Method expressions are not supported!");
        }

        return node;
      }

      private static object? ResolveOwner(Expression expression)
      {
        if (expression == null)
        {
          return null;
        }

        throw new NotImplementedException();
      }

      private static UnaryOperation ConvertUnaryOperator(ExpressionType type) => type switch
      {
        ExpressionType.Not => UnaryOperation.Not,
        
        _ => throw new ArgumentOutOfRangeException(nameof(type))
      };

      private static BinaryOperation ConvertBinaryOperator(ExpressionType type) => type switch
      {
        ExpressionType.Add => BinaryOperation.Add,
        ExpressionType.AddChecked => BinaryOperation.Add,
        ExpressionType.Subtract => BinaryOperation.Subtract,
        ExpressionType.SubtractChecked => BinaryOperation.Subtract,
        ExpressionType.Multiply => BinaryOperation.Multiply,
        ExpressionType.MultiplyChecked => BinaryOperation.Multiply,
        ExpressionType.Divide => BinaryOperation.Divide,

        _ => throw new ArgumentOutOfRangeException(nameof(type))
      };
    }
  }
}