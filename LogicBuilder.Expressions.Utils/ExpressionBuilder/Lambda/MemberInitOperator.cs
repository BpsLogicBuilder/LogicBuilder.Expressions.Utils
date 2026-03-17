using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda
{
    public class MemberInitOperator(IDictionary<string, IExpressionPart> memberBindings, Type? newType = null) : IExpressionPart
    {
        public IDictionary<string, IExpressionPart> MemberBindings { get; } = memberBindings;
        public Type? NewType { get; private set; } = newType;

        public Expression Build() 
            => Build
            (
                MemberBindings.Select
                (
                    binding => new { Name = binding.Key, Expression = binding.Value.Build() }
                ).ToDictionary(k => k.Name, v => v.Expression)
            );

        private Expression Build(IDictionary<string, Expression> bindings)
        {
            if (NewType == null)
            {
                NewType = AnonymousTypeFactory.CreateAnonymousType
                (
                    bindings.ToDictionary(k => k.Key, v => v.Value.Type)
                );
            }

            return Expression.MemberInit
            (
                Expression.New(NewType),
                bindings.Select
                (
                    binding => Expression.Bind(NewType.GetMemberInfo(binding.Key), binding.Value)
                )
            );
        }
    }
}
