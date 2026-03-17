using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators
{
    public class ConvertToNumericTimeOperator(IExpressionPart sourceOperand) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;

        public Expression Build() => Build(SourceOperand.Build());

        private static Expression Build(Expression operandExpression)
        {
            operandExpression = operandExpression.MakeValueSelectorAccessIfNullable();

            if (operandExpression.Type != typeof(DateTimeOffset) 
                && operandExpression.Type != typeof(DateTime) 
                && operandExpression.Type != typeof(TimeSpan)
                && operandExpression.Type.FullName != UnreferencedLiteralTypeNames.TIMEOFDAY
                && operandExpression.Type.FullName != UnreferencedLiteralTypeNames.TIMEONLY)
                return operandExpression;

            return Expression.Add
            (
                Expression.Multiply
                (
                    Expression.Convert(operandExpression.MakeHourSelector(), typeof(long)),
                    Expression.Constant(TimeSpan.TicksPerHour)
                ),
                Expression.Add
                (
                    
                    Expression.Multiply
                    (
                        Expression.Convert(operandExpression.MakeMinuteSelector(), typeof(long)), 
                        Expression.Constant(TimeSpan.TicksPerMinute)
                    ),
                    Expression.Add
                    (
                        Expression.Multiply
                        (
                            Expression.Convert(operandExpression.MakeSecondSelector(), typeof(long)),
                            Expression.Constant(TimeSpan.TicksPerSecond)
                        ),
                        Expression.Convert(operandExpression.MakeMillisecondSelector(), typeof(long))
                    )
                )
            );
        }
    }
}
