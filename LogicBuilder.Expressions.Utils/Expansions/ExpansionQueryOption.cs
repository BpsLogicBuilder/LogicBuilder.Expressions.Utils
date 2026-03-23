using LogicBuilder.Expressions.Utils.Strutures;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class ExpansionQueryOption(SortCollection sortCollection)
    {
        public SortCollection SortCollection { get; } = sortCollection;
    }
}
