namespace CalculationEngine.Model.AST
{
  public interface ICalculationVisitor<out T>
  {
    T Visit(ConstantExpression expression);
    T Visit(UnaryExpression expression);
    T Visit(BinaryExpression expression);
    T Visit(RoundingExpression expression);
    T Visit(ApplyTaxExpression expression);
  }
}