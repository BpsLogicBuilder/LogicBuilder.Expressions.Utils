using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class MaxDateTimeOperatorTest
    {
        [Fact]
        public void MaxDateTimeOperator_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            MaxDateTimeOperator dateOperator = new();

            //act
            var result = dateOperator.Build();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("MaxValue", memberExpr.Member.Name);
        }
    }
}
