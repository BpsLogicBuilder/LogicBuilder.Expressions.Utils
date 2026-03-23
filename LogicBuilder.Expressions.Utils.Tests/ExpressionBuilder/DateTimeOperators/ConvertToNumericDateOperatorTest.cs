using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class ConvertToNumericDateOperatorTest
    {
        [Fact]
        public void ConvertToNumericDateOperator_DateTime_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConvertToNumericDateOperator convertToNumericDateOperator = new(new ConstantOperator(new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Unspecified)));

            // Act
            var expression = convertToNumericDateOperator.Build();

            // Assert
            Assert.IsType<BinaryExpression>(expression, exactMatch: false);
        }

        [Fact]
        public void ConvertToNumericDateOperator_DateTimeOffset_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConvertToNumericDateOperator convertToNumericDateOperator = new(new ConstantOperator(new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero)));

            // Act
            var expression = convertToNumericDateOperator.Build();

            // Assert
            Assert.IsType<BinaryExpression>(expression, exactMatch: false);
        }

        [Fact]
        public void ConvertToNumericDateOperator_ReturnsNoConversionForInvalidType()
        {
            // Arrange
            ConstantOperator constantOperator = new("InvalidType");
            ConvertToNumericDateOperator convertToNumericDateOperator = new(constantOperator);

            // Act
            Expression result = convertToNumericDateOperator.Build();

            // Assert
            var memberExpression = Assert.IsType<MemberExpression>(result, exactMatch: false);
            var constantExpression = (ConstantExpression)memberExpression.Expression;
            Assert.IsType<ConstantContainer>(constantExpression.Value, exactMatch: false);
        }
    }
}
