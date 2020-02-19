using System.Text;

namespace CalculationEngine.Utilities
{
  public static class StringExtensions
  {
    public static StringBuilder AppendWithSeparator(this StringBuilder builder, string value, string seperator)
    {
      if (builder.Length > 0)
      {
        builder.Append(seperator);
      }

      builder.Append(value);

      return builder;
    }
  }
}