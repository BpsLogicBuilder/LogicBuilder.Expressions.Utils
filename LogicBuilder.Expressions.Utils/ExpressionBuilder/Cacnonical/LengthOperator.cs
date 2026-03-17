using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical
{
    public class LengthOperator(IExpressionPart operand) : IExpressionPart
    {
        public IExpressionPart Operand { get; } = operand;

        public Expression Build() => Build(Operand.Build());

        private static Expression Build(Expression operandExpression)
        {
            if (operandExpression.Type.IsList())
                return operandExpression.GetCountCall();
            else if (operandExpression.Type == typeof(string))
                return operandExpression.MakeSelector("Length");
            else
                throw new ArgumentException(nameof(operandExpression));
        }
    }
}
