using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Lambda
{
    public class EnumerableSelectorLambdaOperatorTest
    {
        [Fact]
        public void EnumerableSelectorLambdaOperator_ReturnsCorrectExpression()
        {
            //arrange
            Dictionary<string, System.Linq.Expressions.ParameterExpression> parameters = [];
            EnumerableSelectorLambdaOperator enumerableSelectorLambdaOperator = new
            (
                parameters,
                new ConstantOperator(Enumerable.Empty<Product>()),
                typeof(Product),
                "x"
            );

            //act
            var result = enumerableSelectorLambdaOperator.Build();

            //assert
            Assert.IsType<System.Linq.Expressions.LambdaExpression>(result, exactMatch: false);
            var lambdaExpr = (System.Linq.Expressions.LambdaExpression)result;
            Assert.Equal("x", lambdaExpr.Parameters[0].Name);
        }

        [Fact]
        public void EnumerableSelectorLambdaOperator_ReturnsCorrectExpression_WithGroupingEnumerable()
        {
            //arrange
            Dictionary<string, System.Linq.Expressions.ParameterExpression> parameters = [];
            EnumerableSelectorLambdaOperator enumerableSelectorLambdaOperator = new
            (
                parameters,
                new MemberSelectorOperator("GroupedAddresses", new ParameterOperator(parameters, "x")),
                typeof(Product),
                "x"
            );

            //act
            var result = enumerableSelectorLambdaOperator.Build();

            //assert
            Assert.IsType<System.Linq.Expressions.LambdaExpression>(result, exactMatch: false);
            var lambdaExpr = (System.Linq.Expressions.LambdaExpression)result;
            Assert.Equal("x", lambdaExpr.Parameters[0].Name);
        }
    }
}
