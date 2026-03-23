using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Cacnonical;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Canonical
{
    public class LengthOperatorTest
    {
        [Fact]
        public void LengthOperator_ThrowsForInvalidType()
        {
            //arrange
            LengthOperator lengthOperator = new
            (
                new ParameterOperator(new Dictionary<string, ParameterExpression> { ["q"] = Expression.Parameter(typeof(Product), "q") }, "q")
            );

            //act & assert
            Assert.Throws<ArgumentException>(lengthOperator.Build);
        }
    }
}
