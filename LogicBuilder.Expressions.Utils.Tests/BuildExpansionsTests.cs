using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Expressions.Utils.Tests.Data;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests
{
    public class BuildExpansionsTests
    {
        [Fact]
        public void ShouldNotThrowifExpandedMemberIsLiteral()
        {
            //Arrange Act
            var expression = new SelectExpandDefinition
            {
                ExpandedItems =
                [
                    new SelectExpandItem("Name")
                ]
            }.GetExpansionSelectors<Department>();

            //Assert
            Assert.NotNull(expression);
        }
    }
}
