using System.Reflection;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class NotEqualsBinaryOperatorHandler(IExpressionPart left, IExpressionPart right, FilterFunction @operator) : EqualityBinaryOperatorHandlerBase(left, right, @operator)
    {
        protected override MethodInfo CompareMethodInfo => LinqHelpers.ByteArraysNotEqualMethodInfo;
    }
}
