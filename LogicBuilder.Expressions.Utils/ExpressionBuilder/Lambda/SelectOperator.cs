using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class SelectOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, string selectorParameterName) : SelectorMethodOperatorBase(parameters, sourceOperand, selectorBody, selectorParameterName), IExpressionPart
    {
        protected override Expression Build(Expression operandExpression)
            => operandExpression.GetSelectCall(GetSelector(operandExpression));
    }
}
