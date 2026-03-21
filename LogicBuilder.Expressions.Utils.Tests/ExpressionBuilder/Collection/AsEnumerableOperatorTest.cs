using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Collection;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Collection
{
    public class AsEnumerableOperatorTest
    {
        [Fact]
        public void AsEnumerableOperator_ReturnsCorrectExpression()
        {
            //arrange
            AsEnumerableOperator asEnumerableOperator = new(new ParameterOperator(new Dictionary<string, ParameterExpression> { ["q"] = Expression.Parameter(typeof(IQueryable<Product>), "q") }, "q"));

            //act
            var result = asEnumerableOperator.Build();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            var methodCallExpr = (MethodCallExpression)result;
            Assert.Equal("AsEnumerable", methodCallExpr.Method.Name);
        }
    }
}
