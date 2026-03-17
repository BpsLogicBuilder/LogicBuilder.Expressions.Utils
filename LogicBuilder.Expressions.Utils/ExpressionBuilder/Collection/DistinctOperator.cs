using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection
{
    public class DistinctOperator(IExpressionPart sourceOperand) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;

        public Expression Build() => Build(SourceOperand.Build());

        private static Expression Build(Expression operandExpression)
            => operandExpression.GetDistinctCall();
    }
}
