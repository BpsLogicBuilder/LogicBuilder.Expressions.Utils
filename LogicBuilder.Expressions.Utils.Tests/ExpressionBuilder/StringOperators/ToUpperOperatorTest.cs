using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.StringOperators;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.StringOperators
{
    public class ToUpperOperatorTest
    {
        [Fact]
        public void ToUpperOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantperator = new("abc");
            ToUpperOperator toUpperOperator = new(constantperator);

            // Act
            Expression result = toUpperOperator.Build();

            // Assert
            var resultAsMethodCallExpression = Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToUpper", resultAsMethodCallExpression.Method.Name);
            Assert.Equal("ABC", Expression.Lambda<Func<string>>(result).Compile()());
        }

        [Fact]
        public void ToUpperOperator_ThrowsForInvalidType()
        {
            // Arrange
            ConstantOperator constantperator = new(2);
            ToUpperOperator toUpperOperator = new(constantperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(toUpperOperator.Build);
        }
    }
}
