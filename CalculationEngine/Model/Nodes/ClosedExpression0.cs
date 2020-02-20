using System.Linq.Expressions;
using System.Reflection;
using CalculationEngine.Model.Evaluation;

namespace CalculationEngine.Model.Nodes
{
  internal abstract class ClosedExpression0 : CalculationExpression
  {
    protected abstract decimal Execute(EvaluationContext context);

    internal sealed override decimal Evaluate(EvaluationContext context)
    {
      return Execute(context);
    }

    internal sealed override Expression Compile()
    {
      var method = GetType().GetMethod(nameof(Execute), BindingFlags.NonPublic | BindingFlags.Instance);

      var self    = Expression.Constant(this);
      var context = ContextParameter;

      return Expression.Call(self, method, context);
    }
  }
}