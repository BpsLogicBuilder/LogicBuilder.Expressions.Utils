using LogicBuilder.Expressions.Utils.Expansions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.Expansions
{
    public class SelectsEqualityComparerTest
    {
        [Theory]
        [InlineData("ProductName", "ProductName", true)]
        [InlineData("ProductName", "productName", true)]
        [InlineData("ProductName", "SupplierID", false)]
        [InlineData("ProductName", "supplierID", false)]
        public void SelectsEqualityComparer_Equals_ReturnsExpectedResult(string x, string y, bool expected)
        {
            //arrange
            SelectsEqualityComparer comparer = new();

            //act
            bool result = comparer.Equals(x, y);

            //assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("ProductName", "ProductName", true)]
        [InlineData("ProductName", "productName", false)]
        [InlineData("ProductName", "SupplierID", false)]
        [InlineData("ProductName", "supplierID", false)]
        public void SelectsEqualityComparer_GetHashCode_ReturnsExpectedResult(string x, string y, bool expected)
        {
            //arrange
            SelectsEqualityComparer comparer = new();

            //act
            int hashX = comparer.GetHashCode(x);
            int hashY = comparer.GetHashCode(y);

            //assert
            Assert.Equal(expected, hashX == hashY);
        }
    }
}
