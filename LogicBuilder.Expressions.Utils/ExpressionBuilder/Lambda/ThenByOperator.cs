using LogicBuilder.Expressions.Utils.Strutures;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class ThenByOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, ListSortDirection sortDirection, string selectorParameterName) : SelectorMethodOperatorBase(parameters, sourceOperand, selectorBody, selectorParameterName), IExpressionPart
    {
        public ListSortDirection SortDirection { get; } = sortDirection;
        public override IExpressionPart SelectorBody { get; } = selectorBody;
        public override string SelectorParameterName { get; } = selectorParameterName;
        public override IDictionary<string, ParameterExpression> Parameters { get; } = parameters;

        protected override Expression Build(Expression operandExpression)
            => operandExpression.GetThenByCall(GetSelector(operandExpression), SortDirection);
    }
}
