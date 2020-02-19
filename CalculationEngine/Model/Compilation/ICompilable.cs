using System.Linq.Expressions;

namespace CalculationEngine.Model.Compilation
{
  internal interface ICompilable
  {
    Expression Compile(CompilationContext context);
  }
}