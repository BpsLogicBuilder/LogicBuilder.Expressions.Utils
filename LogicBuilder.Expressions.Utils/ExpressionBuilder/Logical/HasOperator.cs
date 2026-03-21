using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
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
            var rightExpression = Right.Build();
            rightExpression = ConvertRightToEnumExpression
            (
                rightExpression,
                leftExpression
            ) ?? throw new InvalidOperationException($"Unsupported expression type.  The right expression must be a non-null constant.");

            return leftExpression.GetHasFlagCall(rightExpression);
        }

        private static Expression? ConvertRightToEnumExpression(Expression right, Expression left)
        {
            Type leftType = left.Type.ToNullableUnderlyingType();
            if (!leftType.IsEnum)
                throw new ArgumentException(nameof(leftType));

            object? rightValue = GetRightValue();
            if (rightValue == null)
                return null;

            return Expression.Convert
            (
                Expression.Constant
                (
                    Enum.Parse
                    (
                        leftType,
                        rightValue.ToString()
                    ),
                    leftType
                ),
                typeof(Enum)
            );

            object? GetRightValue()
            {
                switch (right)
                {
                    case ConstantExpression constantExpression:
                        return constantExpression.Value;
                    case MemberExpression memberExpression://we use ConstantContainer to prevent SQL injection
                        if (memberExpression.Expression is ConstantExpression memberParent
                            && memberParent.Value is ConstantContainer constantContainer)
                            return constantContainer.Property;

                        throw new InvalidOperationException($"Unable to get value from expression: {memberExpression}.");
                    default:
                        throw new ArgumentException($"Unsupported expression type: {right.Type.FullName}.  The type must be {typeof(ConstantExpression).FullName}.");
                }
            }
        }
    }
}
