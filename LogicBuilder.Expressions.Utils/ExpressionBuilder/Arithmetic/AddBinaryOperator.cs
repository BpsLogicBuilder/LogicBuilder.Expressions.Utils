namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class AddBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.add;
    }
}