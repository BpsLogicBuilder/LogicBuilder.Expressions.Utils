using System;
using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public class ExpansionOptions(string memberName, Type memberType, Type parentType, List<string> selects, ExpansionQueryOption? queryOption = null, ExpansionFilterOption? filterOption = null)
        : Expansion(memberName, memberType, parentType, selects)
    {
        public ExpansionQueryOption? QueryOption { get; set; } = queryOption;
        public ExpansionFilterOption? FilterOption { get; set; } = filterOption;
    }
}
