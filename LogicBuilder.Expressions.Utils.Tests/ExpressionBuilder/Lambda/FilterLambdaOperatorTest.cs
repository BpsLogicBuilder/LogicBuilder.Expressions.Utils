using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Collections.Generic;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Lambda
{
    public class FilterLambdaOperatorTest
    {
        [Fact]
        public void FilterLambdaOperator_ReturnsCorrectExpression()
        {
            //arrange
            Dictionary<string, System.Linq.Expressions.ParameterExpression> parameters = [];
            FilterLambdaOperator filterLambdaOperator = new
            (
                parameters, 
                new EqualsBinaryOperator
                (
                    new MemberSelectorOperator("ProductID", new ParameterOperator(parameters, "x")),
                    new ConstantOperator(1)
                ),
                typeof(Product),
                "x"
            );

            //act
            var result = filterLambdaOperator.Build();

            //assert
            Assert.IsType<System.Linq.Expressions.LambdaExpression>(result, exactMatch: false);
            var lambdaExpr = (System.Linq.Expressions.LambdaExpression)result;
            Assert.Equal("x", lambdaExpr.Parameters[0].Name);
        }
    }
}
