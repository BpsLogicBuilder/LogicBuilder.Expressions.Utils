using LogicBuilder.Expressions.Utils.ExpressionBuilder.Arithmetic;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Arithmetic
{
    public class NegateOperatorTest
    {
        [Fact]
        public void NegateOperator_ReturnsCorrectExpression()
        {
            //arrange
            NegateOperator negateOperator = new(new ConstantOperator(3, type: typeof(int)));

            //act
            var result = negateOperator.Build();

            //assert
            Assert.IsType<UnaryExpression>(result, exactMatch: false);
            var unaryExpr = (UnaryExpression)result;
            Assert.Equal(ExpressionType.Negate, unaryExpr.NodeType);
            Assert.IsType<MemberExpression>(unaryExpr.Operand, exactMatch: false);
            var memberExpression = (MemberExpression)unaryExpr.Operand;
            var constantExpression = (ConstantExpression)memberExpression.Expression;
            var constantContainer = (ConstantContainer)constantExpression.Value;
            Assert.Equal(3, (int)constantContainer.Property);
        }
    }
}
