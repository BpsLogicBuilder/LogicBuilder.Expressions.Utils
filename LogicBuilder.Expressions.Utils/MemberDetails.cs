using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LogicBuilder.Expressions.Utils
{
    public class MemberDetails(Expression selector, string memberName, Type type)
    {
        public Expression Selector { get; set; } = selector;
        public string MemberName { get; set; } = memberName;
        public Type Type { get; set; } = type;
    }
}
