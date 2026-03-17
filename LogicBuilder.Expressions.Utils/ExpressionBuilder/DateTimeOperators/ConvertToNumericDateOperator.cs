using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class ConvertToNumericDateOperator(IExpressionPart sourceOperand) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;

        public Expression Build() => Build(SourceOperand.Build());

        private static Expression Build(Expression operandExpression)
        {
            operandExpression = operandExpression.MakeValueSelectorAccessIfNullable();

            if (operandExpression.Type != typeof(DateTimeOffset) 
                && operandExpression.Type != typeof(DateTime) 
                && operandExpression.Type.FullName != UnreferencedLiteralTypeNames.DATE
                && operandExpression.Type.FullName != UnreferencedLiteralTypeNames.DATEONLY)
                return operandExpression;

            return Expression.Add
            (
                Expression.Add
                (
                    Expression.Multiply
                    (
                        operandExpression.MakeSelector("Year"), 
                        Expression.Constant(10000)
                    ),
                    Expression.Multiply
                    (
                        operandExpression.MakeSelector("Month"), 
                        Expression.Constant(100)
                    )
                ),
                operandExpression.MakeSelector("Day")
            );
        }
    }
}
