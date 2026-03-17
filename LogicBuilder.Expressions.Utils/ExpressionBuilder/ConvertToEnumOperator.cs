using System;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class ConvertToEnumOperator(object constantValue, Type type) : IExpressionPart
    {
        public Type Type { get; } = type;
        public object ConstantValue { get; } = constantValue;

        public Expression Build() => DoBuild();

        private Expression DoBuild()
        {
            if (ConstantValue == null)
                return Expression.Constant(null);

            return ConstantValue.ToString().TryParseEnum(Type, out object? enumValue) 
                ? Expression.Constant(enumValue, Type) 
                : Expression.Constant(null);
        }
    }
}
