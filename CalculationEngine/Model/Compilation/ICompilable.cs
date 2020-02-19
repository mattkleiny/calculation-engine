using System.Linq.Expressions;

namespace CalculationEngine.Model.Compilation
{
  public interface ICompilable
  {
    Expression Compile(CompilationContext context);
  }
}