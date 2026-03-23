using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder
{
    public class ConvertToNullableUnderlyingValueOperatorTest
    {
        [Fact]
        public void ConvertMemberExpressionToNullableUnderlyingValueOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            Dictionary<string, ParameterExpression> parameters = new() { ["a"] = Expression.Parameter(typeof(DataTypes), "a") };
            MemberSelectorOperator memberSelectorOperator = new("NullableUIntProp", new ParameterOperator(parameters, "a"));
            ConvertToNullableUnderlyingValueOperator convertToNullableUnderlyingValueOperator = new(memberSelectorOperator);

            // Act
            Expression result = convertToNullableUnderlyingValueOperator.Build();

            // Assert
            var resultAsMenberExpression = Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal("Value", resultAsMenberExpression.Member.Name);
        }

        [Fact]
        public void ConvertConstantToNullableUnderlyingValueOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            ConstantOperator constantperator = new(1, typeof(int?));
            ConvertToNullableUnderlyingValueOperator convertToNullableUnderlyingValueOperator = new(constantperator);

            // Act
            Expression result = convertToNullableUnderlyingValueOperator.Build();

            // Assert
            var resultAsMenberExpression = Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal("Value", resultAsMenberExpression.Member.Name);
        }

        [Fact]
        public void ConvertToNullableUnderlyingValueOperator_ThrowsIfTypeIsNotNullable()
        {
            // Arrange
            ConstantOperator intOperator = new(1, typeof(int));
            ConvertToNullableUnderlyingValueOperator convertToNullableUnderlyingValueOperator = new(intOperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(convertToNullableUnderlyingValueOperator.Build);
        }
    }
}
