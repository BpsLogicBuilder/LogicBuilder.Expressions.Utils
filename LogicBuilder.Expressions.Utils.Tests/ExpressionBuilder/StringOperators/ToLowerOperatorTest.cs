using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.StringOperators;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.StringOperators
{
    public class ToLowerOperatorTest
    {
        [Fact]
        public void ToLowerOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantperator = new("ABC");
            ToLowerOperator toLowerOperator = new(constantperator);

            // Act
            Expression result = toLowerOperator.Build();

            // Assert
            var resultAsMethodCallExpression = Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToLower", resultAsMethodCallExpression.Method.Name);
            Assert.Equal("abc", Expression.Lambda<Func<string>>(result).Compile()());
        }

        [Fact]
        public void ToLowerOperator_ThrowsForInvalidType()
        {
            // Arrange
            ConstantOperator constantperator = new(2);
            ToLowerOperator toLowerOperator = new(constantperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(toLowerOperator.Build);
        }
    }
}
