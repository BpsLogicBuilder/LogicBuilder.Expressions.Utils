using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class NegateOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;

        public Expression Build()
            => Expression.Negate(this.Operand.Build());
    }
}
