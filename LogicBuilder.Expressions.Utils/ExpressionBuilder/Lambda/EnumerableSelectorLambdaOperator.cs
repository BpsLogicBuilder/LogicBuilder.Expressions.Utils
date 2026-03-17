using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class EnumerableSelectorLambdaOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart selector, Type sourceElementType, string parameterName) : IExpressionPart
    {
        public IExpressionPart Selector { get; } = selector;
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

            var selectorBody = Selector.Build();

            var expression = Expression.Lambda
            (
                typeof(Func<,>).MakeGenericType
                (
                    this.Parameters[ParameterName].Type,
                    typeof(IEnumerable<>).MakeGenericType//specifically using IEnumerable<T> (vs ICollection<T> etc) for the Func return type
                    (
                        GetUnderlyingType(selectorBody)
                    )
                ),
                selectorBody,//don't have to convert the body. The type can remain e.g. ICollection<T>
                this.Parameters[ParameterName]
            );

            this.Parameters.Remove(ParameterName);

            return expression;
        }

        private static Type GetUnderlyingType(Expression expression)
        {
            if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(System.Linq.IGrouping<,>))
                return expression.Type.GetGenericArguments()[1];

            return expression.GetUnderlyingElementType();
        }
    }
}
