namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical
{
    public class OrBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.or;
    }
}
