using LogicBuilder.Expressions.Utils.Expansions;
using LogicBuilder.Expressions.Utils.ExpressionBuilder;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Lambda;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Logical;
using LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand;
using LogicBuilder.Expressions.Utils.Strutures;
using LogicBuilder.Expressions.Utils.Tests.Model;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests.Expansions
{
    public class ExpansionsHelperTest
    {
        [Fact]
        public void GetExpansionSelectors_WithSelect()
        {
            //Arrange
            var expansionDefinition = new SelectExpandDefinition
            (
                ["CategoryID", "CategoryName", "Product"]
            );

            //Act
            var expressions = expansionDefinition.GetExpansionSelectors<CategoryModel>();
            var expressionsToString = expressions.Select(ExpressionStringBuilder.ToString).ToHashSet();

            //Assert
            Assert.Equal(3, expressions.Count());
            Assert.Contains("i => Convert(i.CategoryID)", expressionsToString);
            Assert.Contains("i => i.CategoryName", expressionsToString);
            Assert.Contains("i => i.Product", expressionsToString);
        }

        [Fact]
        public void GetExpansionSelectors_ReturnsEmptyListOfValueTypeSelectorsIfSelectExpandDefinitionIsNull()
        {
            //Act
            var expressions = ExpansionsHelper.GetExpansionSelectors<CategoryModel>(null);

            //Assert
            Assert.NotEmpty(expressions);
        }

        [Fact]
        public void GetExpansions_ReturnsEmptyListIfSelectExpandDefinitionIsNull()
        {
            //Act
            var expressions = ExpansionsHelper.GetExpansions(null, typeof(CategoryModel));

            //Assert
            Assert.Empty(expressions);
        }

        [Fact]
        public void GetExpansionSelectors_FilteringOnChildCollection_AndChildCollectionOfChildCollection_WithMatches()
        {
            //Arrange
            var parameters = new Dictionary<string, ParameterExpression>();
            var expansionDefinition = new SelectExpandDefinition
            (
                null,
                [
                    new SelectExpandItem
                    (
                        "Products",
                        new SelectExpandItemFilter
                        (
                            new FilterLambdaOperator
                            (
                                parameters,
                                new EqualsBinaryOperator
                                (
                                    new MemberSelectorOperator("ProductName", new ParameterOperator(parameters, "a")),
                                    new ConstantOperator("ProductOne")
                                ),
                                typeof(ProductModel),
                                "a"
                            )
                        ),
                        null,
                        null,
                        [
                            new SelectExpandItem
                            (
                                "AlternateAddresses",
                                new SelectExpandItemFilter
                                (
                                    new FilterLambdaOperator
                                    (
                                        parameters,
                                        new EqualsBinaryOperator
                                        (
                                            new MemberSelectorOperator("City", new ParameterOperator(parameters, "b")),
                                            new ConstantOperator("CityOne")
                                        ),
                                        typeof(ProductModel),
                                        "b"
                                    )
                                ),
                                null,
                                null,
                                null
                            )
                        ]
                    )
                ]
            );
    
            //Act
            var expressions = expansionDefinition.GetExpansionSelectors<CategoryModel>();
            var expressionsToString = expressions.Select(ExpressionStringBuilder.ToString).ToHashSet();

            //Assert
            Assert.True(expressions.Any());
            Assert.Contains("i => Convert(i.CategoryID)", expressionsToString);
            Assert.Contains("i => i.CategoryName", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => Convert(i0.ProductID))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.ProductName)", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses)", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => Convert(i1.AddressID)))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => i1.City))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => i1.State))", expressionsToString);
        }

        [Fact]
        public void GetExpansionSelectors_WithSelectsAtRoot_OnChildCollection_AndChildCollectionOfChildCollection()
        {
            //Arrange
            var expansionDefinition = new SelectExpandDefinition
            (
                ["CategoryID", "CategoryName", "Products"],
                [
                    new SelectExpandItem
                    (
                        "Products",
                        null,
                        null,
                        ["ProductID", "ProductName", "AlternateAddresses"],
                        [
                            new SelectExpandItem
                            (
                                "AlternateAddresses",
                                null,
                                null,
                                ["AddressID", "City", "State"],
                                null
                            )
                        ]
                    )
                ]
            );

            //Act
            var expressions = expansionDefinition.GetExpansionSelectors<CategoryModel>();
            var expressionsToString = expressions.Select(ExpressionStringBuilder.ToString).ToHashSet();

            //Assert
            Assert.Equal(9, expressionsToString.Count);
            Assert.Contains("i => Convert(i.CategoryID)", expressionsToString);
            Assert.Contains("i => i.CategoryName", expressionsToString);
            Assert.Contains("i => i.Products", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => Convert(i0.ProductID))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.ProductName)", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses)", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => Convert(i1.AddressID)))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => i1.City))", expressionsToString);
            Assert.Contains("i => i.Products.Select(i0 => i0.AlternateAddresses.Select(i1 => i1.State))", expressionsToString);
        }

        [Fact]
        public void GetExpansionS_FilteringaNDsORTINGOnChildCollection_AndChildCollectionOfChildCollection_WithMatches()
        {
            //Arrange
            var parameters = new Dictionary<string, ParameterExpression>();
            var expansionDefinition = new SelectExpandDefinition
            (
                ["CategoryID", "CategoryName", "Products"],
                [
                    new SelectExpandItem
                    (
                        "Products",
                        new SelectExpandItemFilter
                        (
                            new FilterLambdaOperator
                            (
                                parameters,
                                new EqualsBinaryOperator
                                (
                                    new MemberSelectorOperator("ProductName", new ParameterOperator(parameters, "a")),
                                    new ConstantOperator("ProductOne")
                                ),
                                typeof(ProductModel),
                                "a"
                            )
                        ),
                        new SelectExpandItemQueryFunction
                        (
                            new SortCollection
                            (
                                [
                                    new SortDescription("ProductName", ListSortDirection.Ascending)
                                ],
                                0,
                                20 
                            )
                        ),
                        ["ProductID", "ProductName", "AlternateAddresses"],
                        [
                            new SelectExpandItem
                            (
                                "AlternateAddresses",
                                new SelectExpandItemFilter
                                (
                                    new FilterLambdaOperator
                                    (
                                        parameters,
                                        new EqualsBinaryOperator
                                        (
                                            new MemberSelectorOperator("City", new ParameterOperator(parameters, "b")),
                                            new ConstantOperator("CityOne")
                                        ),
                                        typeof(AddressModel),
                                        "b"
                                    )
                                ),
                                new SelectExpandItemQueryFunction
                                (
                                    new SortCollection
                                    (
                                        [
                                            new SortDescription("City", ListSortDirection.Ascending)
                                        ],
                                        0,
                                        20
                                    )
                                ),
                                ["AddressID", "City", "State"],
                                null
                            )
                        ]
                    )
                ]
            );

            //Act
            var expansions = expansionDefinition.GetExpansions(typeof(CategoryModel));

            //Assert
            Assert.NotEmpty(expansions);
        }
    }
}
