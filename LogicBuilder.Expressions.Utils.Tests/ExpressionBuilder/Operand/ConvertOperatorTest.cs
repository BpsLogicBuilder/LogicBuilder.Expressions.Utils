using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Operand
{
    public class ConvertOperatorTest
    {
        [Fact]
        public void ConvertToNullableUnderlyingValueOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            Dictionary<string, ParameterExpression> parameters = new() { ["a"] = Expression.Parameter(typeof(DataTypes), "a") };
            MemberSelectorOperator memberSelectorOperator = new("NullableUIntProp", new ParameterOperator(parameters, "a"));
            ConvertOperator convertOperator = new(memberSelectorOperator, typeof(object));

            // Act
            Expression result = convertOperator.Build();

            // Assert
            var unaryExpression = Assert.IsType<UnaryExpression>(result, exactMatch: false);
            Assert.Equal(typeof(object), unaryExpression.Type);
        }

        [Fact]
        public void ConvertToInvalidType_ReturnsNullConstantExpression()
        {
            // Arrange
            ConstantOperator intOperator = new(new Product(), typeof(Product));
            ConvertOperator convertOperator = new(intOperator, typeof(int));

            // Act
            Expression result = convertOperator.Build();

            // Assert
            var constantExpression = Assert.IsType<ConstantExpression>(result, exactMatch: false);
            Assert.Null(constantExpression.Value);
        }
    }
}
