using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder
{
    public class ConvertToEnumOperatorTest
    {
        [Theory]
        [InlineData(1, Position.Second)]
        [InlineData(2, Position.Third)]
        [InlineData("eightty", null)]
        [InlineData(null, null)]
        public void ConvertToEnumOperator_ShouldReturnExpectedExpression(object toParse, object expectedResult)
        {
            // Arrange
            ConvertToEnumOperator convertToEnumOperator = new(toParse, typeof(Position));

            // Act
            Expression result = convertToEnumOperator.Build();

            // Assert
            var resultAsConstant = Assert.IsType<ConstantExpression>(result, exactMatch: false);
            Assert.Equal(expectedResult, resultAsConstant.Value);
        }
    }
}
