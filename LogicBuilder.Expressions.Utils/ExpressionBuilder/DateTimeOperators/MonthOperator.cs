using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class MonthOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;

        public Expression Build() => Build(Operand.Build());

        private static Expression Build(Expression operandExpression) 
            => operandExpression.MakeMonthSelector();
    }
}
