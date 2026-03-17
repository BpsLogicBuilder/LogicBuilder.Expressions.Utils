using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection
{
    public class ExceptOperator(IExpressionPart left, IExpressionPart right) : IExpressionPart
    {
        public IExpressionPart Left { get; private set; } = left;
        public IExpressionPart Right { get; private set; } = right;

        public Expression Build() => Build(Left.Build());

        private Expression Build(Expression leftExpression)
        {
            if (leftExpression.Type.IsList())
                return leftExpression.GetExceptCall(Right.Build());
            else
                throw new ArgumentException(nameof(leftExpression));
        }
    }
}
