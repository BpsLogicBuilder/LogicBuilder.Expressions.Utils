using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class TotalSecondsOperatorTest
    {
        [Fact]
        public void TotalSecondsOperator_TimeSpan_ReturnsCorrectExpression()
        {
            //arrange
            TotalSecondsOperator totalSecondsOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(TimeSpan), "dt") }, "dt"));

            //act
            var result = totalSecondsOperator.Build();

            //assert
            Assert.IsType<UnaryExpression>(result, exactMatch: false);
            var unaryExpr = (UnaryExpression)result;
            Assert.Equal("TotalSeconds", ((MemberExpression)unaryExpr.Operand).Member.Name);
        }

        [Fact]
        public void TotalSecondsOperator_ThrowsForInvalidType()
        {
            //arrange
            TotalSecondsOperator totalSecondsOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(string), "dt") }, "dt"));

            //act and assert
            Assert.Throws<ArgumentException>(totalSecondsOperator.Build);
        }
    }
}
