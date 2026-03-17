using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class ExpansionFilterOption(FilterLambdaOperator filterLambdaOperator)
    {
        public FilterLambdaOperator FilterLambdaOperator { get; set; } = filterLambdaOperator;
    }
}
