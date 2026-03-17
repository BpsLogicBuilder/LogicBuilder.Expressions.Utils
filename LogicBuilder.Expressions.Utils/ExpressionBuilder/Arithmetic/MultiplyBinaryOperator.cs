namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class MultiplyBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.mul;
    }
}
