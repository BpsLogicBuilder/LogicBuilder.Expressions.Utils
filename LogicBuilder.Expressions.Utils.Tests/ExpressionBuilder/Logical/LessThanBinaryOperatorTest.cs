using LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using System;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Logical
{
    public class LessThanBinaryOperatorTest
    {
        [Fact]
        public void LessThanBinaryOperator_ReturnsExpectedResultForStringTypes()
        {
            //arrange
            ConstantOperator leftOperator = new("a");
            ConstantOperator rightOperator = new("b");
            LessThanBinaryOperator lessThanBinaryOperator = new(leftOperator, rightOperator);

            //act
            var result = lessThanBinaryOperator.Build();

            //assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
            Assert.True(Expression.Lambda<Func<bool>>(result).Compile()());
        }

        [Fact]
        public void LessThanBinaryOperator_ReturnsExpectedResultForGuildTypes()
        {
            //arrange
            ConstantOperator leftOperator = new(Guid.NewGuid());
            ConstantOperator rightOperator = new(Guid.NewGuid());
            LessThanBinaryOperator lessThanBinaryOperator = new(leftOperator, rightOperator);

            //act
            var result = lessThanBinaryOperator.Build();

            //assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
        }

        [Fact]
        public void LessThanBinaryOperator_ReturnsExpectedResultForNumberTypes()
        {
            //arrange
            ConstantOperator leftOperator = new(1);
            ConstantOperator rightOperator = new(2);
            LessThanBinaryOperator lessThanBinaryOperator = new(leftOperator, rightOperator);

            //act
            var result = lessThanBinaryOperator.Build();

            //assert
            Assert.IsType<BinaryExpression>(result, exactMatch: false);
            Assert.True(Expression.Lambda<Func<bool>>(result).Compile()());
        }
    }
}
