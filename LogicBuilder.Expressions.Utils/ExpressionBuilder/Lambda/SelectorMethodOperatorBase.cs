using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public abstract class SelectorMethodOperatorBase
    {
        protected SelectorMethodOperatorBase(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, string selectorParameterName)
        {
            SourceOperand = sourceOperand;
            SelectorBody = selectorBody;
            SelectorParameterName = selectorParameterName;
            Parameters = parameters;
        }

        protected SelectorMethodOperatorBase(IExpressionPart sourceOperand)
        {
            SourceOperand = sourceOperand;
        }

        public IExpressionPart SourceOperand { get; }
        public IExpressionPart? SelectorBody { get; }
        public string? SelectorParameterName { get; }
        public IDictionary<string, ParameterExpression>? Parameters { get; }

        public Expression Build() => Build(SourceOperand.Build());

        protected abstract Expression Build(Expression operandExpression);

        protected Expression[] GetParameters(Expression operandExpression)
        {
            if (SelectorBody == null 
                || Parameters == null 
                || SelectorParameterName == null)
                return [];

            return
            [
                GetSelector(operandExpression)
            ];
        }

        protected LambdaExpression GetSelector(Expression operandExpression)
            => (LambdaExpression)GetLambdaOperator(operandExpression.GetUnderlyingElementType()).Build();

        protected virtual IExpressionPart GetLambdaOperator(Type elementType)
            => new SelectorLambdaOperator
            (
                Parameters!, // Parameters in not null if GetLambdaOperator is caled
                SelectorBody!,// SelectorBody is not null if GetLambdaOperator is caled
                elementType,
                SelectorParameterName!// SelectorParameterName is not null if GetLambdaOperator is caled
            );
    }
}
