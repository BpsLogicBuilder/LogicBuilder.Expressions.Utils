using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.StringOperators;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.StringOperators
{
    public class ConvertCharArrayToStringOperatorTest
    {
        [Fact]
        public void ConvertCharArrayToStringOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            char[] charArray = { 'H', 'e', 'l', 'l', 'o' };
            string expected = "Hello";
            ConstantOperator charArrayOperator = new(charArray, typeof(char[]));
            ConvertCharArrayToStringOperator convertCharArrayToStringOperator = new(charArrayOperator);

            // Act
            Expression result = convertCharArrayToStringOperator.Build();

            // Assert
            Assert.IsType<NewExpression>(result, exactMatch: false);
            Assert.Equal(typeof(string), result.Type);
            Assert.Equal(expected, Expression.Lambda<Func<string>>(result).Compile()());
        }

        [Fact]
        public void ConvertCharArrayToStringOperator_ThrowsIfOperandIsNotCharArrayExpression()
        {
            // Arrange
            ConstantOperator intOperator = new(1, typeof(int));
            ConvertCharArrayToStringOperator convertCharArrayToStringOperator = new(intOperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(convertCharArrayToStringOperator.Build);
        }
    }
}
