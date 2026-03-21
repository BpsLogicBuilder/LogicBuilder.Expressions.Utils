using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class DateOperatorTest
    {
        [Fact]
        public void MakeDateSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            DateOperator dateOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["dt"]=Expression.Parameter(typeof(DateTime), "dt") }, "dt"));

            //act
            var result = dateOperator.Build();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Date", memberExpr.Member.Name);
        }
    }
}
