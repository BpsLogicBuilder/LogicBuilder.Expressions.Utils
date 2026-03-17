using System.Reflection;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class EqualsBinaryOperatorHandler(IExpressionPart left, IExpressionPart right, FilterFunction @operator) : EqualityBinaryOperatorHandlerBase(left, right, @operator)
    {
        protected override MethodInfo CompareMethodInfo => LinqHelpers.ByteArraysEqualMethodInfo;
    }
}
