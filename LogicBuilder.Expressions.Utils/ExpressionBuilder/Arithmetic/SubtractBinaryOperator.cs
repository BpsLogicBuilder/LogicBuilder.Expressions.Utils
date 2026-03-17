namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class SubtractBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.sub;
    }
}
