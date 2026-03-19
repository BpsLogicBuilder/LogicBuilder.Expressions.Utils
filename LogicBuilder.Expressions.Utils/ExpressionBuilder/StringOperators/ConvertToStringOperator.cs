using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.StringOperators
{
    public class ConvertToStringOperator(IExpressionPart sourceOperand) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;

        public Expression Build() => Build(SourceOperand.Build());

        private static Expression Build(Expression operandExpression)
        {
            if (operandExpression.Type.IsNullableType())
                return ConvertNullable(operandExpression, operandExpression.MakeValueSelectorAccessIfNullable());

            return ConvertNonNullable(operandExpression);
        }

        private static Expression ConvertNullable(Expression operandExpression, Expression underlyingExpression)
        {
            if (underlyingExpression.Type.IsEnum)
                underlyingExpression = ConvertEnumToUnderlyingType(underlyingExpression);

            return Expression.Condition
            (
                operandExpression.MakeHasValueSelector(),
                underlyingExpression.GetObjectToStringCall(),
                Expression.Constant(null, typeof(string))
            );
        }

        private static Expression ConvertNonNullable(Expression operandExpression)
        {
            if (operandExpression.Type.IsEnum)
                operandExpression = ConvertEnumToUnderlyingType(operandExpression);

            return operandExpression.GetObjectToStringCall();
        }

        private static Expression ConvertEnumToUnderlyingType(Expression expression)
            => Expression.Convert(expression, Enum.GetUnderlyingType(expression.Type));
    }
}
