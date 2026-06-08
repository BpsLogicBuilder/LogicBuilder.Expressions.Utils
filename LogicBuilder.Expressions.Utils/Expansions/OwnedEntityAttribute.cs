using System;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class OwnedEntityAttribute : Attribute
    {
    }
}
