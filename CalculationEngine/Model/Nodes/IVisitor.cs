namespace CalculationEngine.Model.Nodes
{
  internal interface IVisitor<out T>
  {
    T Visit(BinaryExpression expression);
    T Visit(ConstantExpression expression);
    T Visit(RoundingExpression expression);
    T Visit(SigmaExpression expression);
    T Visit(TallyExpression expression);
    T Visit(TaxExpression expression);
    T Visit(UnaryExpression expression);
  }
}