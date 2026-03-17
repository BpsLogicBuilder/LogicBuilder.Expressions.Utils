using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand
{
    public class ConvertOperator(IExpressionPart sourceOperand, Type type) : IExpressionPart
    {
        public Type Type { get; } = type;
        public IExpressionPart SourceOperand { get; } = sourceOperand;

        public Expression Build()
        {
            try
            {
                return Expression.Convert(SourceOperand.Build(), Type);
            }
            catch (InvalidOperationException)
            {
                return Expression.Constant(null);
            }
        }
    }
}
