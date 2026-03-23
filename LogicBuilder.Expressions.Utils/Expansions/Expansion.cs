using System;
using System.Collections.Generic;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    abstract public class Expansion(string memberName, Type memberType, Type parentType, List<string> selects)
    {
        public string MemberName { get; } = memberName;
        public Type MemberType { get; } = memberType;
        public Type ParentType { get; } = parentType;
        public List<string> Selects { get; } = selects;
    }
}
