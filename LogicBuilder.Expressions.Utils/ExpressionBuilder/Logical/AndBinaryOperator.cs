namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical
{
    public class AndBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.and;
    }
}
