using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class MaxDateTimeOperator : IExpressionPart
    {
        public Expression Build() => LinqHelpers.GetMaxDateTimOffsetField();
    }
}
