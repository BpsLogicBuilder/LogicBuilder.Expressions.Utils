using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class TimeOperatorTest
    {
        [Fact]
        public void MakeTimeSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            TimeOperator timeOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(DateTime), "dt") }, "dt"));

            //act
            var result = timeOperator.Build();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("TimeOfDay", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeTimeSelector_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            TimeOperator timeOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(DateTimeOffset), "dt") }, "dt"));

            //act
            var result = timeOperator.Build();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("TimeOfDay", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeTimeSelector_ThrowsForInvalidType()
        {
            //arrange
            TimeOperator timeOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"] = Expression.Parameter(typeof(string), "dt") }, "dt"));

            //act and assert
            Assert.Throws<ArgumentException>(timeOperator.Build);
        }
    }
}
