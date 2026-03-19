using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class FilterLambdaOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart filterBody, Type sourceElementType, string parameterName) : IExpressionPart
    {
        public IExpressionPart FilterBody { get; } = filterBody;
        public Type SourceElementType { get; } = sourceElementType;
        public string ParameterName { get; } = parameterName;
        public IDictionary<string, ParameterExpression> Parameters { get; } = parameters;

        public Expression Build()
        {
            this.Parameters.Add
            (
                ParameterName,
                Expression.Parameter(SourceElementType, ParameterName)
            );

            var expression = Expression.Lambda
            (
                typeof(Func<,>).MakeGenericType
                (
                    this.Parameters[ParameterName].Type,
                    typeof(bool)
                ),
                ConvertBody(FilterBody.Build()),
                this.Parameters[ParameterName]
            );

            this.Parameters.Remove(ParameterName);

            return expression;
        }

        private static Expression ConvertBody(Expression body)
            => body.Type != typeof(bool)
                ? Expression.Convert(body, typeof(bool))
                : body;
    }
}
