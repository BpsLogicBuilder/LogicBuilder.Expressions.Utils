using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class SelectExpandItem(string memberName, SelectExpandItemFilter? filter = null, SelectExpandItemQueryFunction? queryFunction = null, List<string>? selects = null, List<SelectExpandItem>? expandedItems = null)
    {
        public string MemberName { get; } = memberName;
        public SelectExpandItemFilter? Filter { get; } = filter;
        public SelectExpandItemQueryFunction? QueryFunction { get; } = queryFunction;
        public List<string> Selects { get; } = selects ?? [];
        public List<SelectExpandItem> ExpandedItems { get; } = expandedItems ?? [];
    }
}
