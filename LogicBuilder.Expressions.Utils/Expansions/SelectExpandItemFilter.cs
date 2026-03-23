using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class SelectExpandItemFilter(FilterLambdaOperator filterLambdaOperator)
    {
        public FilterLambdaOperator FilterLambdaOperator { get; } = filterLambdaOperator;
    }
}
