using LogicBuilder.Expressions.Utils.Strutures;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class SelectExpandItemQueryFunction(SortCollection sortCollection)
    {
        public SortCollection SortCollection { get; set; } = sortCollection;
    }
}
