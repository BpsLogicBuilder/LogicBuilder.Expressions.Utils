using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class HourOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; private set; } = operand;

        public Expression Build() => Build(Operand.Build());

        private static Expression Build(Expression operandExpression) 
            => operandExpression.MakeHourSelector();
    }
}
