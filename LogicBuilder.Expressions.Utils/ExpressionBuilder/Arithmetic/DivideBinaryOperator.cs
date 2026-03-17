namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class DivideBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.div;
    }
}
