using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class NowDateTimeOperator : IExpressionPart
    {
        public Expression Build() => LinqHelpers.GetNowDateTimOffsetProperty();
    }
}
