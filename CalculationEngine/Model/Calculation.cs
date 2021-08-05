using System;
using System.Linq;
using CalculationEngine.Model.Evaluation;
using CalculationEngine.Model.Explanation;
using CalculationEngine.Model.Nodes;
using BinaryExpression = CalculationEngine.Model.Nodes.BinaryExpression;
using ConstantExpression = CalculationEngine.Model.Nodes.ConstantExpression;
using LabelExpression = CalculationEngine.Model.Nodes.LabelExpression;
using UnaryExpression = CalculationEngine.Model.Nodes.UnaryExpression;

namespace CalculationEngine.Model
{
  /// <summary>
  /// Represents a declarative calculation.
  /// <para/>
  /// Calculations are deferred until evaluated, evaluations imply context which permit the
  /// calculations to fetch data from the database and cache results.
  /// <para/>
  /// Calculations are essentially compositions of a simple internal expression tree, and this
  /// struct provides a simple means with which to fluently construct calculations from parts.
  /// </summary>
  public readonly struct Calculation
  {
    /// <summary>Creates a new calculation from the given factory expression</summary>
    public static Calculation Create(Func<Calculation> factory) => factory();

    /// <summary>Sums the given calculation elements, in sequence.</summary>
    public static Calculation Sum(params Calculation[] calculations)
      => new SumExpression(calculations.Select(_ => _.expression));

    /// <summary>Computes the given tax type on the result of the given calculation.</summary>
    public static Calculation Tax(TaxCategory category, Calculation calculation)
      => new TaxExpression(category, calculation.expression);

    /// <summary>Computes the year-to-date values for the given earnings category.</summary>
    public static Calculation YTD(EarningsCategory categories, AnnualisationMethod method = AnnualisationMethod.Weekly)
      => new TallyExpression(categories, method);

    /// <summary>Rounds the given value with the given rounding method.</summary>
    public static Calculation Round(Calculation amount, MidpointRounding rounding = MidpointRounding.AwayFromZero)
      => new RoundingExpression(amount.expression, rounding);

    /// <summary>Truncates the given value.</summary>
    public static Calculation Truncate(Calculation amount)
      => new TruncateExpression(amount.expression);

    /// <summary>Labels the given part of the calculation.</summary>
    public static Calculation Label(string label, Calculation calculation)
      => new LabelExpression(label, calculation.expression);

    /// <summary>Memoizes the result of the given calculation, storing it in a variable and optionally labelling it.</summary>
    public static Calculation Variable(string name, Calculation calculation, bool includeLabel = true)
      => new VariableExpression(name, calculation.expression, includeLabel);

    private readonly CalculationExpression expression;

    internal Calculation(CalculationExpression expression)
    {
      this.expression = expression;
    }

    /// <summary>
    /// Evaluates the calculation and returns it's final output.
    /// Evaluations might make use of database or cache resources to fetch and compose data.
    /// </summary>
    public decimal Evaluate(EvaluationContext context)
    {
      return expression.Evaluate(context);
    }

    /// <summary>
    /// Produces a <see cref="CalculationExplanation"/> which details the individual steps of the
    /// calculation. This explanation could be used in UI rendering, or just for breaking a calculation
    /// down to better understand it.
    /// <para/>
    /// The <see cref="EvaluationContext"/> is required here, as each step is capable of evaluating it's
    /// cumulative figure as part of the total.
    /// </summary>
    public CalculationExplanation Explain(EvaluationContext context)
    {
      var explanation = new ExplanationContext(context);

      expression.Explain(explanation);

      return explanation.ToExplanation();
    }

    public override string ToString()
    {
      return expression.ToString();
    }

    public static Calculation operator -(Calculation a)                => new UnaryExpression(UnaryOperation.Negate, a.expression);
    public static Calculation operator +(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Add, a.expression, b.expression);
    public static Calculation operator -(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Subtract, a.expression, b.expression);
    public static Calculation operator *(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Multiply, a.expression, b.expression);
    public static Calculation operator /(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Divide, a.expression, b.expression);
    public static Calculation operator %(Calculation a, Calculation b) => new BinaryExpression(BinaryOperation.Modulo, a.expression, b.expression);

    public static implicit operator Calculation(decimal value) => new(new ConstantExpression(value));
  }
}