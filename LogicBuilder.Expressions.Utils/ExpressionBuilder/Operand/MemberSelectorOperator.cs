using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand
{
    public class MemberSelectorOperator(string memberFullName, IExpressionPart sourceOperand) : IExpressionPart
    {
        public string MemberFullName { get; set; } = memberFullName;
        public IExpressionPart SourceOperand { get; set; } = sourceOperand;

        public Expression Build() 
            => SourceOperand.Build().BuildSelectorExpression(MemberFullName);
    }
}
