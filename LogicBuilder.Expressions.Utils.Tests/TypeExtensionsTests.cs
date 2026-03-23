using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests
{
    public class TypeExtensionsTests
    {
        [Theory]
        [InlineData(nameof(DerivedThing.Id), typeof(DerivedThing))]
        [InlineData(nameof(DerivedThing.Name), typeof(BaseThing))]
        [InlineData(nameof(DerivedThing.Description), typeof(DerivedThing))]
        public void MemberInfoReflectedTypeMustMatchTheDeclaringType(string propertyName, Type reflectedType)
        {
            //act
            MemberInfo memberInfo = typeof(DerivedThing).GetMemberInfo(propertyName);

            //assert
            Assert.Equal(reflectedType.FullName, memberInfo.ReflectedType.FullName);
        }

        [Theory]
        [InlineData(nameof(DerivedThing.Id), typeof(DerivedThing))]
        [InlineData(nameof(DerivedThing.Name), typeof(BaseThing))]
        [InlineData(nameof(DerivedThing.Description), typeof(DerivedThing))]
        public void MemberInfoReflectedTypeMustMatchTheDeclaringTypeForGetSelectedMembers(string propertyName,
            Type reflectedType)
        {
            //act
            MemberInfo memberInfo = typeof(DerivedThing).GetSelectedMembers([])
                .FirstOrDefault(m => m.Name == propertyName);

            //assert
            Assert.Equal(reflectedType.FullName, memberInfo.ReflectedType.FullName);
        }

        [Fact]
        public void GetSelectedMembers_WhenSelectIsEmpty_MustReturnAllLiteralAndLiteralListMembers()
        {
            // Act
            var memberInfos = typeof(Thing).GetSelectedMembers(Enumerable.Empty<string>().ToList());

            // Assert
            Assert.Multiple(() =>
            {
                var names = memberInfos.Select(mi => mi.Name).ToList();
                
                Assert.DoesNotContain(memberInfos, name => name.Name == nameof(Thing.Objects));
                
                Assert.Contains(names, name => name == nameof(Thing.ParametersArray));
                Assert.Contains(names, name => name == nameof(Thing.ParametersList));
                Assert.Contains(names, name => name == nameof(Thing.Ints));
                Assert.Contains(names, name => name == nameof(Thing.Strings));
                Assert.Contains(names, name => name == nameof(Thing.Booleans));
                Assert.Contains(names, name => name == nameof(Thing.DateTimes));
                Assert.Contains(names, name => name == nameof(Thing.Dates));
                Assert.Contains(names, name => name == nameof(Thing.Guides));
                Assert.Contains(names, name => name == nameof(Thing.UnsignedInts));
                Assert.Contains(names, name => name == nameof(Thing.Name));
                Assert.Contains(names, name => name == nameof(Thing.Id));
                Assert.Contains(names, name => name == nameof(Thing.Description));
            });
        }

        [Theory]
        [InlineData(typeof(Product), "ProductName", typeof(string))]
        [InlineData(typeof(Product), "Category.CategoryName", typeof(string))]
        [InlineData(typeof(BaseThing), "BaseId", typeof(int))]
        [InlineData(typeof(BaseThing), "GetHashCode", typeof(int))]
        public void GetMemberInfoFromFullName_MustReturnTheTypeOfTheMember(Type declaringType, string memberName, Type expectedMemberType)
        {
            //arrange
            MemberInfo memberInfo = declaringType.GetMemberInfoFromFullName(memberName);

            //act
            Type memberType = memberInfo.GetMemberType();

            //assert
            Assert.Equal(expectedMemberType, memberType);
        }

        [Fact]
        public void GetMemberInfo_ThrowsForInvalidName()
        {
            //act & assert
            Assert.Throws<ArgumentException>(() => typeof(Product).GetMemberInfo("InvalidName"));
        }

        [Theory]
        [InlineData(typeof(Product), "ProductName", typeof(string))]
        [InlineData(typeof(BaseThing), "BaseId", typeof(int))]
        [InlineData(typeof(BaseThing), "BaseTypeId", typeof(int))]
        [InlineData(typeof(BaseThing), "GetHashCode", typeof(int))]
        public void GetMemberTypeFromMemberInfo_MustReturnTheTypeOfTheMember(Type declaringType, string memberName, Type expectedMemberType)
        {
            //arrange
            MemberInfo memberInfo = declaringType.GetMemberInfo(memberName);

            //act
            Type memberType = memberInfo.GetMemberType();

            //assert
            Assert.Equal(expectedMemberType, memberType);
        }

        [Theory]
        [InlineData(typeof(Product), "ProductName", typeof(string))]
        [InlineData(typeof(BaseThing), "BaseId", typeof(int))]
        public void GetMemberTypeFromMemberExpression_MustReturnTheTypeOfTheMember(Type declaringType, string memberName, Type expectedMemberType)
        {
            //arrange
            ParameterExpression parameterExpression = Expression.Parameter(declaringType, "x");
            MemberInfo memberInfo = declaringType.GetMemberInfo(memberName);
            MemberExpression memberExpression = Expression.MakeMemberAccess(parameterExpression, memberInfo);

            //act
            Type memberType = memberExpression.GetMemberType();

            //assert
            Assert.Equal(expectedMemberType, memberType);
        }

        [Fact]
        public void GetMemberTypeFromMemberInfo_ThrowsForInvalidMemberType()
        {
            //arrange
            MemberInfo memberInfo = typeof(Product);

            //act
            Assert.Throws<ArgumentOutOfRangeException>(memberInfo.GetMemberType);
        }

        [Fact]
        public void GetMemberTypeFromMemberInfo_ThrowsWhenmemberInfoIsNull()
        {
            //arrange
            MemberInfo memberInfo = null;

            //act
            Assert.Throws<ArgumentNullException>(memberInfo.GetMemberType);
        }

        [Theory]
        [InlineData("First", typeof(Position), true)]
        [InlineData("Second", typeof(Position), true)]
        [InlineData("Second", typeof(Position?), true)]
        [InlineData("Fifth", typeof(Position), false)]
        [InlineData("Fifth", typeof(Position?), false)]
        public void TryParseEnum_MustReturnExpectedResult(string value, Type enumType, bool expectedResult)
        {
            //act
            bool result = value.TryParseEnum(enumType, out _);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(Product))]
        public void TryParseEnum_ThrowsForInvalidType(Type enumType)
        {
            //act
            Assert.Throws<ArgumentException>(() => "".TryParseEnum(enumType, out _));
        }

        [Theory]
        [InlineData(typeof(Product), typeof(Product))]
        [InlineData(typeof(int), typeof(int?))]
        [InlineData(typeof(int?), typeof(int?))]
        public void ToNullable_ReturnsTheExprectedType(Type type, Type expectedResultType)
        {
            //act
            Type resultType = type.ToNullable();

            //assert
            Assert.Equal(expectedResultType, resultType);
        }

        [Theory]
        [InlineData(typeof(List<Product>), typeof(Product))]
        [InlineData(typeof(int[]), typeof(int))]
        public void GetUnderlyingElementType_ReturnsTheExprectedType(Type type, Type expectedResultType)
        {
            //act
            Type resultType = type.GetUnderlyingElementType();

            //assert
            Assert.Equal(expectedResultType, resultType);
        }

        [Theory]
        [InlineData(typeof(IDictionary<int, Product>))]
        [InlineData(typeof(Product))]
        public void GetUnderlyingElementType_ThrowsForInvalidType(Type type)
        {
            //act & assert
            Assert.Throws<ArgumentException>(type.GetUnderlyingElementType);
        }

        [Theory]
        [InlineData(typeof(Product), new string[] { "ProductID", "ProductName" }, 2)]
        [InlineData(typeof(Product), null, 31)]
        [InlineData(typeof(List<Product>), null, 0)]
        public void GetValueTypeMembers_WithSelects_ReturnsExprectedMembers(Type type, string[] selects, int expectedCount)
        {
            //act
            MemberInfo[] memberInfos = type.GetSelectedMembers(selects == null ? [] : [.. selects]);

            //assert
            Assert.Equal(expectedCount, memberInfos.Length);
        }

#pragma warning disable S1144 // Remove unused property (used for testing purposes)
        private abstract class BaseThing
        {
            public string Name { get; set; }//NOSONAR - used for testing purposes
            public int BaseId { get; set; }//NOSONAR - used for testing purposes
#pragma warning disable CS0649 // Remove unused property (used for testing purposes)
            public int BaseTypeId;//is never assigned to, and will always have its default value 0 NOSONAR - used for testing purposes
#pragma warning restore CS0649
        }

        private class DerivedThing : BaseThing, IDerivedThing
        {
            public Guid Id { get; set; }//NOSONAR - used for testing purposes
            public string Description { get; set; }
        }

        private interface IDerivedThing
        {
            public string Description { get; set; }
        }

        private class Thing
        {
            public string Name { get; set; }//NOSONAR - used for testing purposes
            public Guid Id { get; set; }//NOSONAR - used for testing purposes
            public string Description { get; set; }//NOSONAR - used for testing purposes
            public byte[] DataInBytes { get; set; }//NOSONAR - used for testing purposes
            public string[] ParametersArray { get; set; }//NOSONAR - used for testing purposes
            public ICollection<string> Strings { get; set; }//NOSONAR - used for testing purposes
            public List<string> ParametersList { get; set; }//NOSONAR - used for testing purposes
            public List<bool> Booleans { get; set; }//NOSONAR - used for testing purposes
            public ISet<DateTime> DateTimes { get; set; }//NOSONAR - used for testing purposes
            public ISet<DateOnly> Dates { get; set; }//NOSONAR - used for testing purposes
            public HashSet<Guid> Guides { get; set; }//NOSONAR - used for testing purposes
            public uint[] UnsignedInts { get; set; }//NOSONAR - used for testing purposes
            public IEnumerable<int> Ints { get; set; }//NOSONAR - used for testing purposes
            public List<object> Objects { get; set; }//NOSONAR - used for testing purposes
        }
#pragma warning restore S1144 // Remove unused property
    }
}