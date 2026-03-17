using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class ParameterOperator(IDictionary<string, ParameterExpression> parameters, string parameterName) : IExpressionPart
    {
        public IDictionary<string, ParameterExpression> Parameters { get; } = parameters;
        public string ParameterName { get; } = parameterName;

        public Expression Build() => Parameters[ParameterName];
    }
}
