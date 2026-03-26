using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class SelectManyOperator(IDictionary<string, ParameterExpression> parameters, IExpressionPart sourceOperand, IExpressionPart selectorBody, string selectorParameterName) : SelectorMethodOperatorBase(parameters, sourceOperand, selectorBody, selectorParameterName), IExpressionPart
    {
        public override IExpressionPart SelectorBody { get; } = selectorBody;
        public override string SelectorParameterName { get; } = selectorParameterName;
        public override IDictionary<string, ParameterExpression> Parameters { get; } = parameters;

        protected override Expression Build(Expression operandExpression)
            => operandExpression.GetSelectManyCall(GetSelector(operandExpression));

        protected override IExpressionPart GetLambdaOperator(Type elementType)
            => new EnumerableSelectorLambdaOperator
            (
                Parameters!,// Parameters in not null if GetLambdaOperator is caled
                SelectorBody!,// SelectorBody is not null if GetLambdaOperator is caled
                elementType,
                SelectorParameterName!// SelectorParameterName is not null if GetLambdaOperator is caled
            );
    }
}
