using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder
{
    public class CustomMethodOperator(MethodInfo methodInfo, IExpressionPart[] args) : IExpressionPart
    {
        public MethodInfo MethodInfo { get; } = methodInfo;
        public IExpressionPart[] Args { get; } = args;

        public Expression Build() => Build(Args.Select(arg => arg.Build()));

        private Expression Build(IEnumerable<Expression> arguments) 
            => MethodInfo.IsStatic
                ? Expression.Call(MethodInfo, arguments)
                : Expression.Call(arguments.First(), MethodInfo, arguments.Skip(1));
    }
}
