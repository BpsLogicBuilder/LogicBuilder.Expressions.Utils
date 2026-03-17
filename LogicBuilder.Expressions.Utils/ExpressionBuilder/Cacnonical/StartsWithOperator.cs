using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical
{
    public class StartsWithOperator(IExpressionPart left, IExpressionPart right) : IExpressionPart
    {
        public IExpressionPart Left { get; } = left;
        public IExpressionPart Right { get; } = right;

        public Expression Build() => Build(Left.Build());

        private Expression Build(Expression leftExpression)
        {
            if (leftExpression.Type == typeof(string))
                return leftExpression.GetStringStartsWithCall(Right.Build());
            else
                throw new ArgumentException(nameof(leftExpression));
        }
    }
}
