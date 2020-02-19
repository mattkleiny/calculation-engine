using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.AST;
using CalculationEngine.Model.Compilation;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using BinaryExpression = CalculationEngine.Model.AST.BinaryExpression;
using ConstantExpression = CalculationEngine.Model.AST.ConstantExpression;
using UnaryExpression = CalculationEngine.Model.AST.UnaryExpression;

namespace CalculationEngine.Model
{
  // TODO: allow parsing graphs from text?
  // TODO: add a fluent 'builder' pattern on top of this

  public delegate decimal Calculation(EvaluationContext context);

  public sealed class CalculationGraph
  {
    private readonly CalculationExpression expression;

    public static CalculationGraph FromExpression(Expression<Calculation> expression)
    {
      var visitor = new TranslationVisitor();

      visitor.Visit(expression);

      var result = visitor.Expressions.Dequeue();

      return new CalculationGraph(result);
    }

    public CalculationGraph(CalculationExpression expression)
    {
      this.expression = expression;
    }

    public decimal Evaluate(EvaluationContext context)
    {
      return expression.Evaluate(context);
    }

    public CalculationExplanation ToExplanation(EvaluationContext context)
    {
      var explanation = new ExplanationContext(context);

      expression.Explain(explanation);

      return new CalculationExplanation(explanation.Steps);
    }

    public Expression ToLinqExpression()
    {
      var context = new CompilationContext();

      return expression.Compile(context);
    }

    public Calculation ToDelegate()
    {
      var expression = ToLinqExpression();
      var invocation = Expression.Lambda(expression).Compile();

      return context => (decimal) invocation.DynamicInvoke();
    }

    public override string ToString()
    {
      return expression.ToString();
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