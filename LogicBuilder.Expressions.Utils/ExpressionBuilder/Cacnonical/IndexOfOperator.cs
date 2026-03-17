using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical
{
    public class IndexOfOperator(IExpressionPart sourceOperand, IExpressionPart itemToFind) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;
        public IExpressionPart ItemToFind { get; } = itemToFind;

        public Expression Build() => Build(SourceOperand.Build());

        private Expression Build(Expression leftExpression)
        {
            if (leftExpression.Type == typeof(string))
                return leftExpression.GetStringIndexOfCall(ItemToFind.Build());
            else
                throw new ArgumentException(nameof(leftExpression));
        }
    }
}
