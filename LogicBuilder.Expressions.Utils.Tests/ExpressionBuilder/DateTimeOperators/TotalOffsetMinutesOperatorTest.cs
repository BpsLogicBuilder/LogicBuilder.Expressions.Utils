using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class TotalOffsetMinutesOperatorTest
    {
        [Fact]
        public void TotalOffsetMinutesOperator_TimeSpan_ReturnsCorrectExpression()
        {
            //arrange
            TotalOffsetMinutesOperator totalOffsetMinutesOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(DateTimeOffset), "dt") }, "dt"));

            //act
            var result = totalOffsetMinutesOperator.Build();

            //assert
            Assert.IsType<UnaryExpression>(result, exactMatch: false);
            var unaryExpr = (UnaryExpression)result;
            Assert.Equal("TotalMinutes", ((MemberExpression)unaryExpr.Operand).Member.Name);
        }

        [Fact]
        public void TotalOffsetMinutesOperator_ThrowsForInvalidType()
        {
            //arrange
            TotalOffsetMinutesOperator totalOffsetMinutesOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(string), "dt") }, "dt"));

            //act and assert
            Assert.Throws<ArgumentException>(totalOffsetMinutesOperator.Build);
        }
    }
}
