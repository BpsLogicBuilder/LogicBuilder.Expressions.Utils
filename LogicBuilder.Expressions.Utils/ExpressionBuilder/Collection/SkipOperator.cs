using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection
{
    public class SkipOperator(IExpressionPart sourceOperand, int count) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;
        public int Count { get; } = count;

        public Expression Build() => Build(SourceOperand.Build());

        private Expression Build(Expression operandExpression)
            => operandExpression.GetSkipCall(Count);
    }
}
