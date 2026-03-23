using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Collection
{
    public class UnionOperatorTest
    {
        [Fact]
        public void UnionOperator_ThrowsWhenLeftExpressionIsNotAList()
        {
            //arrange
            UnionOperator unionOperator = new
            (
                new ParameterOperator(new Dictionary<string, ParameterExpression> { ["q"] = Expression.Parameter(typeof(Product), "q") }, "q"),
                new ConstantOperator(null)
            );

            //act & assert
            Assert.Throws<ArgumentException>(unionOperator.Build);
        }
    }
}
