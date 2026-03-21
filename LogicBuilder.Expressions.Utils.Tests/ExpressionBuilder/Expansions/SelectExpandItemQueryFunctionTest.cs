using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Expressions.Utils.Strutures;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.ExpressionBuilder.Expansions
{
    public class SelectExpandItemQueryFunctionTest
    {
        [Fact]
        public void ExpansionQueryOption_CreatesExpectedExpression()
        {
            //arrange
            SelectExpandItemQueryFunction queryFunction = new
            (
                new SortCollection
                (
                    [
                        new SortDescription("ProductName", ListSortDirection.Ascending),
                        new SortDescription("SupplierID", ListSortDirection.Descending),
                        new SortDescription("DateProperty", ListSortDirection.Ascending)
                    ]
                )
            );

            ParameterExpression param = Expression.Parameter(typeof(IQueryable<Product>), "q");

            //act
            Expression mce = param.GetOrderBy<Product>(queryFunction.SortCollection);

            //assert
            Assert.NotNull(mce);
            MethodCallExpression mceTake = Assert.IsType<MethodCallExpression>(mce, exactMatch: false);
            MethodCallExpression mceSkip = Assert.IsType<MethodCallExpression>(mceTake.Arguments[0], exactMatch: false);
            MethodCallExpression mceThenBy = Assert.IsType<MethodCallExpression>(mceSkip.Arguments[0], exactMatch: false);
            MethodCallExpression mceThenByDescending = Assert.IsType<MethodCallExpression>(mceThenBy.Arguments[0], exactMatch: false);
            MethodCallExpression mceOrderBy = Assert.IsType<MethodCallExpression>(mceThenByDescending.Arguments[0], exactMatch: false);
            Assert.Equal("ThenBy", mceThenBy.Method.Name);
            Assert.Equal("ThenByDescending", mceThenByDescending.Method.Name);
            Assert.Equal("OrderBy", mceOrderBy.Method.Name);
        }
    }
}
