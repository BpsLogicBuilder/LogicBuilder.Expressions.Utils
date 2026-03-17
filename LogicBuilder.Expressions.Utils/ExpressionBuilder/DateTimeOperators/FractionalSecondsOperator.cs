using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class FractionalSecondsOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;

        public Expression Build() => Build(Operand.Build());

        private static Expression Build(Expression operandExpression) 
            => GetFractionalSeconds(operandExpression.MakeMillisecondSelector());

        private static Expression GetFractionalSeconds(Expression milliseconds) 
            => Expression.Divide
            (
                Expression.Convert(milliseconds, typeof(decimal)),
                Expression.Constant(1000m, typeof(decimal))
            );
    }
}
