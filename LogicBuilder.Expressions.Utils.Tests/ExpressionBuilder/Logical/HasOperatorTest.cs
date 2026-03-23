using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Logical
{
    public class HasOperatorTest
    {
        [Fact]
        public void HasOperator_ShouldReturnExpectedExpression()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            System.IO.FileAttributes rightValue = System.IO.FileAttributes.ReadOnly;
            bool expected = true;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            ConstantOperator rightOperator = new(rightValue, typeof(System.IO.FileAttributes));
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act
            var resultExpression = hasOperator.Build();
            var resultFunc = Expression.Lambda<Func<bool>>(resultExpression).Compile();

            // Assert
            Assert.Equal(expected, resultFunc());
        }

        [Fact]
        public void HasOperator_ShouldReturnExpectedExpression_WhenRightSideDoesNotUseConstantContainer()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            bool expected = true;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            ConvertToEnumOperator rightOperator = new(1, typeof(System.IO.FileAttributes));
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act
            var resultExpression = hasOperator.Build();
            var resultFunc = Expression.Lambda<Func<bool>>(resultExpression).Compile();

            // Assert
            Assert.Equal(expected, resultFunc());
        }

        [Fact]
        public void HasOperator_ShouldReturnExpectedExpressionWhenRightSideIsInteger()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            bool expected = true;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            ConstantOperator rightOperator = new(1, typeof(int));
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act
            var resultExpression = hasOperator.Build();
            var resultFunc = Expression.Lambda<Func<bool>>(resultExpression).Compile();

            // Assert
            Assert.Equal(expected, resultFunc());
        }

        [Fact]
        public void HasOperator_ThrowsIfLeftOperandIsNotEnumExpression()
        {     
            // Arrange
            ConstantOperator leftOperator = new(1, typeof(int));
            ConstantOperator rightOperator = new(1, typeof(int));
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act & Assert
            Assert.Throws<ArgumentException>(hasOperator.Build);
        }

        [Fact]
        public void HasOperator_ThrowsWhenRightSideIsNotConstantOrMemberExpression()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            ParameterOperator rightOperator = new(new Dictionary<string, ParameterExpression> { { "a", Expression.Parameter(typeof(Product), "a") } }, "a");
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(hasOperator.Build);
            Assert.Equal($"Unsupported expression type: {typeof(Product).FullName}.  The type must be {typeof(ConstantExpression).FullName}.", exception.Message);
        }

        [Fact]
        public void HasOperator_ThrowsWhenRightSideIsUnsuppotedMemberExpression()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            ParameterOperator rightOperator = new(new Dictionary<string, ParameterExpression> { { "a", Expression.Parameter(typeof(Product), "a") } }, "a");
            MemberSelectorOperator memberSelectorOperator = new("ProductID", rightOperator);
            HasOperator hasOperator = new(leftOperator, memberSelectorOperator);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(hasOperator.Build);
            Assert.Contains($"Unable to get value from expression:", exception.Message);
        }

        [Fact]
        public void HasOperator_ThrowsIfRightSideIsNull()
        {
            // Arrange
            System.IO.FileAttributes leftValue = System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden;
            ConstantOperator leftOperator = new(leftValue, typeof(System.IO.FileAttributes));
            int? rightValue = null;
            ConstantOperator rightOperator = new(rightValue, typeof(int?));
            HasOperator hasOperator = new(leftOperator, rightOperator);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(hasOperator.Build);
            Assert.Contains("Unsupported expression type", exception.Message);
        }
    }
}
