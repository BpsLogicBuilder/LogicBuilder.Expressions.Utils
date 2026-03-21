using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class BinaryOperatorHandler(IExpressionPart left, IExpressionPart right, FilterFunction @operator)
    {
        public virtual FilterFunction Operator { get; } = @operator;
        public IExpressionPart Left { get; } = left;
        public IExpressionPart Right { get; } = right;

        public virtual Expression Build()
        {
            var leftExpression = Left.Build();
            var rightExpression = Right.Build();

            MatchTypes(ref leftExpression, ref rightExpression);

            return Build(leftExpression, rightExpression);
        }

        protected virtual Expression Build(Expression left, Expression right)
            => Expression.MakeBinary
            (
                Constants.BinaryOperatorExpressionType[Operator],
                left,
                right
            );

        protected static void MatchTypes(ref Expression left, ref Expression right)
        {
            if (left.Type == right.Type)
                return;

            left = left.ToNullable();
            right = right.ToNullable();
        }
    }
}
