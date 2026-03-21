using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class AllOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart filterBody, string filterParameterName) : FilterMethodOperatorBase(parameters, sourceOperand, filterBody, filterParameterName), IExpressionPart
    {
        protected override Expression Build(Expression operandExpression)
            => operandExpression.GetAllCall(GetParameters(operandExpression));
    }
}
