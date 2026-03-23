using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Canonical
{
    public class StartsWithOperatorTest
    {
        [Fact]
        public void StartsWithOperator_ThrowsWhenLeftExpressionTypeIsNotString()
        {
            //arrange
            StartsWithOperator startsWithOperator = new
            (
                new ParameterOperator(new Dictionary<string, ParameterExpression> { ["q"] = Expression.Parameter(typeof(Product), "q") }, "q"),
                new ConstantOperator(null)
            );

            //act & assert
            Assert.Throws<ArgumentException>(startsWithOperator.Build);
        }
    }
}
