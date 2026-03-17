using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class TotalOffsetMinutesOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; private set; } = operand;

        public Expression Build() => Build(Operand.Build());

        private static Expression Build(Expression operandExpression)
        {
            operandExpression = operandExpression.MakeValueSelectorAccessIfNullable();

            if (operandExpression.Type == typeof(DateTimeOffset))
                return Expression.Convert
                (
                    operandExpression.MakeSelector("Offset.TotalMinutes"), 
                    typeof(int)
                );
            else
                throw new ArgumentException(nameof(Operand));
        }
    }
}
