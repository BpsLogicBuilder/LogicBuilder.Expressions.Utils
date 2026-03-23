using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class SelectExpandDefinition(List<string>? selects = null, List<SelectExpandItem>? expandedItems = null)
    {
        public List<string> Selects { get; } = selects ?? [];
        public List<SelectExpandItem> ExpandedItems { get; } = expandedItems ?? [];
    }
}
