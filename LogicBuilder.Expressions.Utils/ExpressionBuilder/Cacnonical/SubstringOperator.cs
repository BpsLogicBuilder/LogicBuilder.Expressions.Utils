using System;
using System.Linq;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical
{
    public class SubstringOperator(IExpressionPart sourceOperand, params IExpressionPart[] indexes) : IExpressionPart
    {
        public IExpressionPart SourceOperand { get; } = sourceOperand;
        public IExpressionPart[] Indexes { get; } = indexes;

        public Expression Build() => Build(SourceOperand.Build());

        private Expression Build(Expression leftExpression)
        {
            if (leftExpression.Type == typeof(string))
                return leftExpression.GetSubStringCall
                (
                    [.. Indexes.Select(arg => arg.Build())]
                );
            else
                throw new ArgumentException(nameof(Indexes));
        }
    }
}
