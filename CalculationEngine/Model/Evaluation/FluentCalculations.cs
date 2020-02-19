using System;
using System.Dynamic;
using System.Linq.Expressions;
using CalculationEngine.Model.Tree;
using BinaryExpression = CalculationEngine.Model.Tree.BinaryExpression;
using ConstantExpression = CalculationEngine.Model.Tree.ConstantExpression;
using UnaryExpression = CalculationEngine.Model.Tree.UnaryExpression;

namespace CalculationEngine.Model.Evaluation
{
  public static class FluentCalculations
  {
    public static dynamic Tax(TaxCategory category, dynamic expression)
      => ToDynamic(new TaxExpression(TaxOperation.Calculate, category, Reify(expression)));

    public static dynamic YTD(EarningsCategory category)
      => ToDynamic(new TallyExpression(category));

    private static DynamicCalculationBuilder ToDynamic(CalculationExpression expression)
      => new DynamicCalculationBuilder(expression);

    private static dynamic Reify(dynamic expression)
    {
      if (expression is decimal)
      {
        return new ConstantExpression(expression);
      }

      if (expression is CalculationExpression)
      {
        return expression;
      }

      if (expression is DynamicCalculationBuilder builder)
      {
        return builder.Expression;
      }

      throw new ArgumentOutOfRangeException(nameof(expression));
    }

    private sealed class DynamicCalculationBuilder : DynamicObject
    {
      public CalculationExpression Expression { get; }

      public DynamicCalculationBuilder(CalculationExpression expression)
      {
        Expression = expression;
      }

      public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
      {
        switch (binder.Operation)
        {
          case ExpressionType.Not:
            result = ToDynamic(new UnaryExpression(UnaryOperation.Not, Expression));
            return true;
        }

        return base.TryUnaryOperation(binder, out result);
      }

      public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
      {
        switch (binder.Operation)
        {
          case ExpressionType.Add:
            result = ToDynamic(new BinaryExpression(BinaryOperation.Add, Expression, Reify(arg)));
            return true;

          case ExpressionType.Subtract:
            result = ToDynamic(new BinaryExpression(BinaryOperation.Subtract, Expression, Reify(arg)));
            return true;

          case ExpressionType.Multiply:
            result = ToDynamic(new BinaryExpression(BinaryOperation.Multiply, Expression, Reify(arg)));
            return true;

          case ExpressionType.Divide:
            result = ToDynamic(new BinaryExpression(BinaryOperation.Divide, Expression, Reify(arg)));
            return true;

          default:
            return base.TryBinaryOperation(binder, arg, out result);
        }
      }

      public override bool TryConvert(ConvertBinder binder, out object result)
      {
        if (binder.ReturnType == typeof(decimal))
        {
          result = Expression.Evaluate(new EvaluationContext());
          return true;
        }

        if (typeof(CalculationExpression).IsAssignableFrom(binder.ReturnType))
        {
          result = Expression;
          return true;
        }

        return base.TryConvert(binder, out result);
      }
    }
  }
}