using LogicBuilder.Expressions.Utils.Strutures;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class ThenByOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, ListSortDirection sortDirection, string selectorParameterName) : SelectorMethodOperatorBase(parameters, sourceOperand, selectorBody, selectorParameterName), IExpressionPart
    {
        public ListSortDirection SortDirection { get; } = sortDirection;

        protected override Expression Build(Expression operandExpression)
            => operandExpression.GetThenByCall(GetSelector(operandExpression), SortDirection);
    }
}
