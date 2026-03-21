using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class MinDateTimeOperatorTest
    {
        [Fact]
        public void MinDateTimeOperator_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            MinDateTimeOperator dateOperator = new();

            //act
            var result = dateOperator.Build();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("MinValue", memberExpr.Member.Name);
        }
    }
}
