using LogicBuilder.Expressions.Utils.ExpressionBuilder.DateTimeOperators;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.DateTimeOperators
{
    public class ConvertToNumericTimeOperatorTest
    {
        [Fact]
        public void ConvertToNumericTimeOperator_DateTime_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantOperator = new(new DateTime(2024, 1, 1, 12, 30, 0, DateTimeKind.Unspecified));
            ConvertToNumericTimeOperator convertToNumericTimeOperator = new(constantOperator);

            // Act
            Expression result = convertToNumericTimeOperator.Build();

            // Assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
        }

        [Fact]
        public void ConvertToNumericTimeOperator_DateTimeOffset_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantOperator = new(new DateTimeOffset(2024, 1, 1, 12, 30, 0, TimeSpan.Zero));
            ConvertToNumericTimeOperator convertToNumericTimeOperator = new(constantOperator);

            // Act
            Expression result = convertToNumericTimeOperator.Build();

            // Assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
        }

        [Fact]
        public void ConvertToNumericTimeOperator_TimeSpan_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantOperator = new(new TimeSpan(12, 30, 0));
            ConvertToNumericTimeOperator convertToNumericTimeOperator = new(constantOperator);

            // Act
            Expression result = convertToNumericTimeOperator.Build();

            // Assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
        }

        [Fact]
        public void ConvertToNumericTimeOperator_ReturnsNoConversionForInvalidType()
        {
            // Arrange
            ConstantOperator constantOperator = new("InvalidType");
            ConvertToNumericTimeOperator convertToNumericTimeOperator = new(constantOperator);

            // Act
            Expression result = convertToNumericTimeOperator.Build();

            // Assert
            var memberExpression = Assert.IsType<MemberExpression>(result, exactMatch: false);
            var constantExpression = (ConstantExpression)memberExpression.Expression;
            Assert.IsType<ConstantContainer>(constantExpression.Value, exactMatch: false);
        }
    }
}
