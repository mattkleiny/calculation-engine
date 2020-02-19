using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Tree;
using BinaryExpression = System.Linq.Expressions.BinaryExpression;
using ConstantExpression = System.Linq.Expressions.ConstantExpression;
using UnaryExpression = System.Linq.Expressions.UnaryExpression;

namespace CalculationEngine.Model.Parsing.Linq
{
  internal static class LinqParser
  {
    public static CalculationExpression Parse(Expression<Func<decimal>> expression)
    {
      var visitor = new TransformationVisitor();

      visitor.Visit(expression);

      return visitor.Expressions.Dequeue();
    }

    private sealed class TransformationVisitor : ExpressionVisitor
    {
      public Queue<CalculationExpression> Expressions { get; } = new Queue<CalculationExpression>();

      protected override Expression VisitConstant(ConstantExpression node)
      {
        base.VisitConstant(node);

        Expressions.Enqueue(new Tree.ConstantExpression(Convert.ToDecimal(node.Value)));

        return node;
      }

      protected override Expression VisitUnary(UnaryExpression node)
      {
        base.VisitUnary(node);

        var type = ConvertUnaryOperator(node.NodeType);

        Expressions.Enqueue(new Tree.UnaryExpression(type, Expressions.Dequeue()));

        return node;
      }

      protected override Expression VisitBinary(BinaryExpression node)
      {
        base.VisitBinary(node);

        var type = ConvertBinaryOperator(node.NodeType);

        Expressions.Enqueue(new Tree.BinaryExpression(type, Expressions.Dequeue(), Expressions.Dequeue()));

        return node;
      }

      protected override Expression VisitMember(MemberExpression node)
      {
        base.VisitMember(node);

        var owner = ResolveOwner(node.Expression);

        switch (node.Member)
        {
          case FieldInfo field:
            Expressions.Enqueue(new Tree.ConstantExpression(Convert.ToDecimal(field.GetValue(owner))));
            break;

          case PropertyInfo property:
            Expressions.Enqueue(new Tree.ConstantExpression(Convert.ToDecimal(property.GetValue(owner))));
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