using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class WhereOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart filterBody, string filterParameterName) : FilterMethodOperatorBase(parameters, sourceOperand, filterBody, filterParameterName), IExpressionPart
    {
        public override IExpressionPart FilterBody { get; } = filterBody;
        public override IDictionary<string, ParameterExpression> Parameters { get; } = parameters;
        public override string FilterParameterName { get; } = filterParameterName;

        protected override Expression Build(Expression operandExpression) 
            => operandExpression.GetWhereCall(GetParameters(operandExpression));
    }
}
