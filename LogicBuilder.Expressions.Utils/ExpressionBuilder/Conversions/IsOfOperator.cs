using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Conversions
{
    public class IsOfOperator(IExpressionPart operand, Type type) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;
        public Type Type { get; } = type;

        public Expression Build() => Build(Operand.Build());

        private Expression Build(Expression operandExpression)
            => Expression.Condition
            (
                Expression.TypeIs(operandExpression, Type), 
                Expression.Constant(true), 
                Expression.Constant(false)
            );
    }
}
