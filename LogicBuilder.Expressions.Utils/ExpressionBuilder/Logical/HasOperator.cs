using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical
{
    public class HasOperator(IExpressionPart left, IExpressionPart right) : IExpressionPart
    {
        public IExpressionPart Left { get; } = left;
        public IExpressionPart Right { get; } = right;

        public Expression Build()
        {
            var leftExpression = Left.Build();
            
            return leftExpression.GetHasFlagCall
            (
                ConvertRightToEnumExpression
                (
                    Right.Build(),
                    leftExpression.Type.ToNullableUnderlyingType()
                )
            );
        }

        private static Expression ConvertRightToEnumExpression(Expression right, Type leftType)
        {
            if (!leftType.IsEnum)
                throw new ArgumentException(nameof(leftType));

            return Expression.Convert
            (
                Expression.Constant
                (
                    Enum.Parse
                    (
                        leftType,
                        ((ConstantExpression)right).Value.ToString()
                    ),
                    leftType
                ), 
                typeof(Enum)
            );
        }
    }
}
