using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Conversions
{
    public class CastOperator(IExpressionPart operand, System.Type type) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;
        public System.Type Type { get; } = type;

        public Expression Build() => Build(Operand.Build());

        private Expression Build(Expression operandExpression) 
            => Expression.TypeAs(operandExpression, Type);
    }
}
