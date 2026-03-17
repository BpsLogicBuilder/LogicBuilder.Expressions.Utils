using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class MinDateTimeOperator : IExpressionPart
    {
        public Expression Build() => LinqHelpers.GetMinDateTimOffsetField();
    }
}
