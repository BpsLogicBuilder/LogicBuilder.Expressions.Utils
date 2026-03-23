using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.StringOperators;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.StringOperators
{
    public class TrimOperatorTest
    {
        [Fact]
        public void TrimOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantperator = new("ABC  ");
            TrimOperator trimOperator = new(constantperator);

            // Act
            Expression result = trimOperator.Build();

            // Assert
            var resultAsMethodCallExpression = Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Trim", resultAsMethodCallExpression.Method.Name);
            Assert.Equal("ABC", Expression.Lambda<Func<string>>(result).Compile()());
        }

        [Fact]
        public void TrimOperator_ThrowsForInvalidType()
        {
            // Arrange
            ConstantOperator constantperator = new(2);
            TrimOperator trimOperator = new(constantperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(trimOperator.Build);
        }
    }
}
