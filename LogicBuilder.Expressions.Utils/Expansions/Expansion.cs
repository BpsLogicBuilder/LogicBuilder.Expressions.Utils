using System;
using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    abstract public class Expansion(string memberName, Type memberType, Type parentType, List<string> selects)
    {
        public string MemberName { get; set; } = memberName;
        public Type MemberType { get; set; } = memberType;
        public Type ParentType { get; set; } = parentType;
        public List<string> Selects { get; set; } = selects;
    }
}
