using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand
{
    public class CollectionConstantOperator(ICollection<object> constantValues, Type elementType) : IExpressionPart
    {
        public Type ElementType { get; } = elementType;
        public ICollection<object> ConstantValues { get; } = constantValues;

        public Expression Build()
        {
            Type listType = typeof(List<>).MakeGenericType(ElementType);
            IList items = (IList)Activator.CreateInstance(listType);

            foreach (object next in ConstantValues)
                items.Add(next);

            return Expression.Constant(items, listType);
        }
    }
}
