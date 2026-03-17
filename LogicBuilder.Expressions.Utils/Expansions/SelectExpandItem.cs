using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class SelectExpandItem(string memberName, SelectExpandItemFilter? filter = null, SelectExpandItemQueryFunction? queryFunction = null, List<string>? selects = null, List<SelectExpandItem>? expandedItems = null)
    {
        public string MemberName { get; set; } = memberName;
        public SelectExpandItemFilter? Filter { get; set; } = filter;
        public SelectExpandItemQueryFunction? QueryFunction { get; set; } = queryFunction;
        public List<string> Selects { get; set; } = selects ?? [];
        public List<SelectExpandItem> ExpandedItems { get; set; } = expandedItems ?? [];
    }
}
