using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection
{
    public class TakeOperator(IExpressionPart sourceOperand, int count) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;
        public int Count { get; } = count;

        public Expression Build() => Build(SourceOperand.Build());

        private Expression Build(Expression operandExpression)
            => operandExpression.GetTakeCall(Count);
    }
}
