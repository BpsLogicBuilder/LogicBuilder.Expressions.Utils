using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical
{
    public class InOperator(IExpressionPart itemToFind, IExpressionPart listToSearch) : IExpressionPart
    {
        public IExpressionPart ItemToFind { get; private set; } = itemToFind;
        public IExpressionPart ListToSearch { get; private set; } = listToSearch;

        public Expression Build()
            => ListToSearch.Build().GetContainsCall(ItemToFind.Build());
    }
}
