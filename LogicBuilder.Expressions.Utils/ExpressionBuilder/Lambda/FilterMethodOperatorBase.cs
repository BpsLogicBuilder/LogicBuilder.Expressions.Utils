using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public abstract class FilterMethodOperatorBase
    {
        protected FilterMethodOperatorBase(IDictionary<string, ParameterExpression>? parameters, IExpressionPart sourceOperand, IExpressionPart? filterBody, string? filterParameterName)
        {
            SourceOperand = sourceOperand;
            FilterBody = filterBody;
            Parameters = parameters;
            FilterParameterName = filterParameterName;
        }

        protected FilterMethodOperatorBase(IExpressionPart sourceOperand)
        {
            SourceOperand = sourceOperand;
        }

        public IExpressionPart SourceOperand { get; }
        public virtual IExpressionPart? FilterBody { get; }
        public virtual IDictionary<string, ParameterExpression>? Parameters { get; }
        public virtual string? FilterParameterName { get; }

        public Expression Build() => Build(SourceOperand.Build());

        protected abstract Expression Build(Expression operandExpression);

        protected Expression[] GetParameters(Expression operandExpression)
        {
            if (FilterBody == null 
                || FilterParameterName == null 
                || Parameters == null)
                return [];

            return
            [
                GetFilterLambdaOperator(operandExpression.GetUnderlyingElementType()).Build()
            ];
        }

        protected FilterLambdaOperator GetFilterLambdaOperator(Type elementType) 
            => new
            (
                Parameters!,// Parameters is not null if GetFilterLambdaOperator is called.
                FilterBody!,// FilterBody is not null if GetFilterLambdaOperator is called.
                elementType,
                FilterParameterName!// FilterParameterName is not null if GetFilterLambdaOperator is called.
            );
    }
}
