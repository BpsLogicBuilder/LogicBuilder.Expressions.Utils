namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic
{
    public class ModuloBinaryOperator(IExpressionPart left, IExpressionPart right) : BinaryOperator(left, right)
    {
        public override FilterFunction Operator => FilterFunction.mod;
    }
}
