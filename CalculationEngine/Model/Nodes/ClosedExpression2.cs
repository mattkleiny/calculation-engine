using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.Nodes
{
  internal abstract class ClosedExpression2 : CalculationExpression
  {
    protected abstract CalculationExpression Parameter1 { get; }
    protected abstract CalculationExpression Parameter2 { get; }

    protected abstract decimal Execute(EvaluationContext context, decimal param1, decimal param2);

    internal sealed override decimal Evaluate(EvaluationContext context)
    {
      var param1 = Parameter1.Evaluate(context);
      var param2 = Parameter2.Evaluate(context);

      return Execute(context, param1, param2);
    }

    internal sealed override Expression Compile()
    {
      var method = GetType().GetMethod(nameof(Execute), BindingFlags.NonPublic);

      var self    = Expression.Constant(this);
      var context = ContextParameter;
      var param1  = Parameter1.Compile();
      var param2  = Parameter2.Compile();

      return Expression.Call(self, method, context, param1, param2);
    }
  }
}