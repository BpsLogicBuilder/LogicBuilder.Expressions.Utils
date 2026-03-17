using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Conversions
{
    public class CollectionOfTypeOperator(IExpressionPart operand, Type type) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;
        public Type Type { get; } = type;

        public Expression Build() => Build(Operand.Build());

        private Expression Build(Expression operandExpression)
            => operandExpression.GetOfTypeCall(Type);
    }
}
