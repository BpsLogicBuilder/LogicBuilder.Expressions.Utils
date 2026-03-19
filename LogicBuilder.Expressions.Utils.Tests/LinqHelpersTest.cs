using LogicBuilder.Expressions.Utils.Strutures;
using LogicBuilder.Expressions.Utils.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace LogicBuilder.Expressions.Utils.Tests
{
    public class LinqHelpersTest
    {
        #region BuildSelectorExpression Tests
        [Fact]
        public void BuildSelectorExpression_SimpleProperty_ReturnsCorrectExpression()
        {
            //arrange
            string propertyName = "AlternateAddresses.AddressId";
            ParameterExpression param = Expression.Parameter(typeof(Product), "p");

            //act
            var result = LinqHelpers.BuildSelectorExpression(param, propertyName);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal(typeof(IEnumerable<int>), result.Type);
        }
        #endregion

        #region GetTypedSelector Tests
        [Fact]
        public void GetTypedSelector_Generic_ReturnsCorrectLambdaExpression()
        {
            //arrange
            string propertyName = "ProductName";

            //act
            var selector = propertyName.GetTypedSelector<Product>();

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, string>), selector.Type);
            Assert.Single(selector.Parameters);
            Assert.Equal("a", selector.Parameters[0].Name);
        }

        [Fact]
        public void GetTypedSelector_WithCustomParameterName_UsesCustomName()
        {
            //arrange
            string propertyName = "ProductName";

            //act
            var selector = propertyName.GetTypedSelector<Product>("p");

            //assert
            Assert.Equal("p", selector.Parameters[0].Name);
        }

        [Fact]
        public void GetTypedSelector_NestedProperty_ReturnsCorrectSelector()
        {
            //arrange
            string propertyName = "Category.CategoryName";

            //act
            var selector = propertyName.GetTypedSelector<Product>();

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, string>), selector.Type);
        }

        [Fact]
        public void GetTypedSelector_NonGeneric_ReturnsCorrectLambdaExpression()
        {
            //arrange
            string propertyName = "ProductName";
            Type parentType = typeof(Product);

            //act
            var selector = propertyName.GetTypedSelector(parentType);

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, string>), selector.Type);
        }
        #endregion

        #region GetObjectSelector Tests
        [Fact]
        public void GetObjectSelector_Generic_ValueType_ConvertsToObject()
        {
            //arrange
            string propertyName = "ProductID";

            //act
            var selector = propertyName.GetObjectSelector<Product>();

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, object>), selector.Type);
        }

        [Fact]
        public void GetObjectSelector_Generic_ReferenceType_ReturnsObject()
        {
            //arrange
            string propertyName = "ProductName";

            //act
            var selector = propertyName.GetObjectSelector<Product>();

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, object>), selector.Type);
        }

        [Fact]
        public void GetObjectSelector_NonGeneric_ReturnsCorrectSelector()
        {
            //arrange
            string propertyName = "ProductID";
            Type parentType = typeof(Product);

            //act
            var selector = propertyName.GetObjectSelector(parentType);

            //assert
            Assert.NotNull(selector);
            Assert.Equal(typeof(Func<Product, object>), selector.Type);
        }
        #endregion

        #region MakeValueSelectorAccessIfNullable Tests
        [Fact]
        public void MakeValueSelectorAccessIfNullable_NullableType_ReturnsValueSelector()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(int?), "p");

            //act
            var result = param.MakeValueSelectorAccessIfNullable();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
        }

        [Fact]
        public void MakeValueSelectorAccessIfNullable_NonNullableType_ReturnsOriginalExpression()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(int), "p");

            //act
            var result = param.MakeValueSelectorAccessIfNullable();

            //assert
            Assert.Same(param, result);
        }
        #endregion

        #region MakeHasValueSelector Tests
        [Fact]
        public void MakeHasValueSelector_NullableType_ReturnsHasValueSelector()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(int?), "p");

            //act
            var result = param.MakeHasValueSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(bool), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("HasValue", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHasValueSelector_NonNullableType_ThrowsArgumentException()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(int), "p");

            //act & assert
            Assert.Throws<ArgumentException>(() => param.MakeHasValueSelector());
        }
        #endregion

        #region MakeSelector Tests
        [Fact]
        public void MakeSelector_SimpleProperty_ReturnsCorrectExpression()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(Product), "p");

            //act
            var result = param.MakeSelector("ProductName");

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(string), result.Type);
        }

        [Fact]
        public void MakeSelector_NestedProperty_ReturnsCorrectExpression()
        {
            //arrange
            ParameterExpression param = Expression.Parameter(typeof(Product), "p");

            //act
            var result = param.MakeSelector("Category.CategoryName");

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(string), result.Type);
        }
        #endregion

        #region ChildParameterName Tests
        [Fact]
        public void ChildParameterName_WithNumericSuffix_IncrementsNumber()
        {
            //arrange
            string parameterName = "p0";

            //act
            var result = parameterName.ChildParameterName();

            //assert
            Assert.Equal("p1", result);
        }

        [Fact]
        public void ChildParameterName_WithoutNumericSuffix_AppendsZero()
        {
            //arrange
            string parameterName = "param";

            //act
            var result = parameterName.ChildParameterName();

            //assert
            Assert.Equal("param0", result);
        }

        [Fact]
        public void ChildParameterName_MultiDigitNumber_IncrementsCorrectly()
        {
            //arrange
            string parameterName = "p9";

            //act
            var result = parameterName.ChildParameterName();

            //assert
            Assert.Equal("p10", result);
        }
        #endregion

        #region SetNullType Tests
        [Fact]
        public void SetNullType_ConstantWithNull_ReturnsTypedConstant()
        {
            //arrange
            Expression expr = Expression.Constant(null);

            //act
            var result = expr.SetNullType(typeof(string));

            //assert
            Assert.IsType<ConstantExpression>(result, exactMatch: false);
            Assert.Equal(typeof(string), result.Type);
            Assert.Null(((ConstantExpression)result).Value);
        }

        [Fact]
        public void SetNullType_ConstantWithValue_ReturnsOriginalExpression()
        {
            //arrange
            Expression expr = Expression.Constant("test");

            //act
            var result = expr.SetNullType(typeof(string));

            //assert
            Assert.Same(expr, result);
        }

        [Fact]
        public void SetNullType_NonConstant_ReturnsOriginalExpression()
        {
            //arrange
            Expression expr = Expression.Parameter(typeof(string), "p");

            //act
            var result = expr.SetNullType(typeof(object));

            //assert
            Assert.Same(expr, result);
        }
        #endregion

        #region Date/Time Selector Tests
        [Fact]
        public void MakeDaySelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Day", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeDaySelector_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTimeOffset), "dt");

            //act
            var result = param.MakeDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
        }

        [Fact]
        public void MakeDaySelector_Date_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(Microsoft.OData.Edm.Date), "dt");

            //act
            var result = param.MakeDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
        }

        [Fact]
        public void MakeDaySelector_DateOnly_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateOnly), "dt");

            //act
            var result = param.MakeDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
        }

        [Fact]
        public void MakeDaySelector_NullableDateTime_AccessesValueFirst()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime?), "dt");

            //act
            var result = param.MakeDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
        }

        [Fact]
        public void MakeMonthSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeMonthSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Month", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMonthSelector_Date_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(Microsoft.OData.Edm.Date), "dt");

            //act
            var result = param.MakeMonthSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Month", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMonthSelector_DateOnly_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateOnly), "dt");

            //act
            var result = param.MakeMonthSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Month", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMonthSelector_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTimeOffset), "dt");

            //act
            var result = param.MakeMonthSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Month", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeYearSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeYearSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Year", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeYearSelector_Date_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(Microsoft.OData.Edm.Date), "dt");

            //act
            var result = param.MakeYearSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Year", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeYearSelector_DateOnly_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateOnly), "dt");

            //act
            var result = param.MakeYearSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Year", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeYearSelector_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTimeOffset), "dt");

            //act
            var result = param.MakeYearSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Year", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHourSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeHourSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Hour", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHourSelector_DateTimeOffset_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTimeOffset), "dt");

            //act
            var result = param.MakeHourSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Hour", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHourSelector_TimeOnly_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(TimeOnly), "dt");

            //act
            var result = param.MakeHourSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(int), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Hour", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHourSelector_TimeSpan_ReturnsHoursProperty()
        {
            //arrange
            var param = Expression.Parameter(typeof(TimeSpan), "ts");

            //act
            var result = param.MakeHourSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Hours", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeHourSelector_TimeOfDay_ReturnsHoursProperty()
        {
            //arrange
            var param = Expression.Parameter(typeof(Microsoft.OData.Edm.TimeOfDay), "ts");

            //act
            var result = param.MakeHourSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Hours", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMinuteSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeMinuteSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Minute", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMinuteSelector_TimeSpan_ReturnsMinutesProperty()
        {
            //arrange
            var param = Expression.Parameter(typeof(TimeSpan), "ts");

            //act
            var result = param.MakeMinuteSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Minutes", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeSecondSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeSecondSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Second", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeSecondSelector_TimeSpan_ReturnsSecondsProperty()
        {
            //arrange
            var param = Expression.Parameter(typeof(TimeSpan), "ts");

            //act
            var result = param.MakeSecondSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Seconds", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMillisecondSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeMillisecondSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Millisecond", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeMillisecondSelector_TimeSpan_ReturnsMillisecondsProperty()
        {
            //arrange
            var param = Expression.Parameter(typeof(TimeSpan), "ts");

            //act
            var result = param.MakeMillisecondSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Milliseconds", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeDateSelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeDateSelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("Date", memberExpr.Member.Name);
        }

        [Fact]
        public void MakeTimeOfDaySelector_DateTime_ReturnsCorrectExpression()
        {
            //arrange
            var param = Expression.Parameter(typeof(DateTime), "dt");

            //act
            var result = param.MakeTimeOfDaySelector();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("TimeOfDay", memberExpr.Member.Name);
        }
        #endregion

        #region String Method Call Tests
        [Fact]
        public void GetStringContainsCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var operand = Expression.Constant("World");

            //act
            var result = instance.GetStringContainsCall(operand);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Contains", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringStartsWithCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var operand = Expression.Constant("Hello");

            //act
            var result = instance.GetStringStartsWithCall(operand);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("StartsWith", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringEndsWithCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var operand = Expression.Constant("World");

            //act
            var result = instance.GetStringEndsWithCall(operand);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("EndsWith", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringIndexOfCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var operand = Expression.Constant("World");

            //act
            var result = instance.GetStringIndexOfCall(operand);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("IndexOf", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSubStringCall_OneParameter_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var startIndex = Expression.Constant(6);

            //act
            var result = instance.GetSubStringCall(startIndex);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Substring", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSubStringCall_TwoParameters_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("Hello World");
            var startIndex = Expression.Constant(0);
            var length = Expression.Constant(5);

            //act
            var result = instance.GetSubStringCall(startIndex, length);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Substring", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringToLowerCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("HELLO");

            //act
            var result = instance.GetStringToLowerCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToLower", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringToUpperCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("hello");

            //act
            var result = instance.GetStringToUpperCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToUpper", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringTrimCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant("  hello  ");

            //act
            var result = instance.GetStringTrimCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Trim", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetObjectToStringCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant(123);

            //act
            var result = instance.GetObjectToStringCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToString", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringConcatCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var left = Expression.Constant("Hello");
            var right = Expression.Constant("World");

            //act
            var result = LinqHelpers.GetStringConcatCall(left, right);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Concat", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetStringCompareCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var left = Expression.Constant("A");
            var right = Expression.Constant("B");

            //act
            var result = LinqHelpers.GetStringCompareCall(left, right);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Compare", ((MethodCallExpression)result).Method.Name);
        }
        #endregion

        #region LINQ Method Call Tests
        [Fact]
        public void GetWhereCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var predicate = Expression.Lambda<Func<Product, bool>>(
                Expression.Constant(true),
                param
            );

            //act
            var result = queryable.GetWhereCall(predicate);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Where", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSelectCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, string>>(
                Expression.Property(param, "ProductName"),
                param
            );

            //act
            var result = queryable.GetSelectCall(selector);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Select", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetAnyCall_WithoutPredicate_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetAnyCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Any", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetCountCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetCountCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Count", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetFirstCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Product() }.AsQueryable());

            //act
            var result = queryable.GetFirstCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("First", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetFirstOrDefaultCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetFirstOrDefaultCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("FirstOrDefault", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSkipCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetSkipCall(10);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Skip", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetTakeCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetTakeCall(10);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Take", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetDistinctCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetDistinctCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Distinct", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetOrderByCall_WithAscendingSort_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, string>>(
                Expression.Property(param, "ProductName"),
                param
            );

            //act
            var result = queryable.GetOrderByCall(selector, ListSortDirection.Ascending);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("OrderBy", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetOrderByCall_WithDescendingSort_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, string>>(
                Expression.Property(param, "ProductName"),
                param
            );

            //act
            var result = queryable.GetOrderByCall(selector, ListSortDirection.Descending);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("OrderByDescending", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetThenByCall_WithAscendingSort_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, string>>(
                Expression.Property(param, "ProductName"),
                param
            );

            //act
            var result = queryable.GetThenByCall(selector, ListSortDirection.Ascending);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ThenBy", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetGroupByCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, int>>(
                Expression.Property(param, "ProductID"),
                param
            );

            //act
            var result = queryable.GetGroupByCall(selector);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("GroupBy", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetToListCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var enumerable = Expression.Constant(Enumerable.Empty<Product>());

            //act
            var result = enumerable.GetToListCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("ToList", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetAsQueryableCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var enumerable = Expression.Constant(Enumerable.Empty<Product>(), typeof(IEnumerable<Product>));

            //act
            var result = enumerable.GetAsQueryableCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("AsQueryable", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetAsEnumerableCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetAsEnumerableCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("AsEnumerable", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetCastCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var enumerable = Expression.Constant(Enumerable.Empty<object>());

            //act
            var result = enumerable.GetCastCall(typeof(Product));

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Cast", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetOfTypeCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var enumerable = Expression.Constant(Enumerable.Empty<object>());

            //act
            var result = enumerable.GetOfTypeCall(typeof(Product));

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("OfType", ((MethodCallExpression)result).Method.Name);
        }
        #endregion

        #region Math Method Call Tests
        [Fact]
        public void GetCeilingCall_Decimal_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7m);

            //act
            var result = operand.GetCeilingCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Ceiling", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetCeilingCall_Double_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7);

            //act
            var result = operand.GetCeilingCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Ceiling", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetRoundCall_Decimal_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7m);

            //act
            var result = operand.GetRoundCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Round", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetRoundCall_Double_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7);

            //act
            var result = operand.GetRoundCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Round", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetFloorCall_Decimal_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7m);

            //act
            var result = operand.GetFloorCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Floor", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetFloorCall_Double_ReturnsCorrectMethodCall()
        {
            //arrange
            var operand = Expression.Constant(3.7);

            //act
            var result = operand.GetFloorCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Floor", ((MethodCallExpression)result).Method.Name);
        }

        private static readonly int[] sourceArray123 = [1, 2, 3];
        private static readonly int[] sourceArray23 = [2, 3];
        private static readonly int[] sourceArray12 = [1, 2];
        private static readonly int[] sourceArray34 = [3, 4];
        #endregion

        #region Aggregate Method Call Tests
        [Fact]
        public void GetAverageCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(sourceArray123.AsQueryable());

            //act
            var result = queryable.GetAverageCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Average", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetMaxCall_WithoutSelector_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(sourceArray123.AsQueryable());

            //act
            var result = queryable.GetMaxCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Max", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetMaxCall_WithSelector_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Product() }.AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, int>>(
                Expression.Property(param, "ProductID"),
                param
            );

            //act
            var result = queryable.GetMaxCall(selector);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Max", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetMinCall_WithoutSelector_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(sourceArray123.AsQueryable());

            //act
            var result = queryable.GetMinCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Min", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetMinCall_WithSelector_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Product() }.AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var selector = Expression.Lambda<Func<Product, int>>(
                Expression.Property(param, "ProductID"),
                param
            );

            //act
            var result = queryable.GetMinCall(selector);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Min", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSumCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(sourceArray123.AsQueryable());

            //act
            var result = queryable.GetSumCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Sum", ((MethodCallExpression)result).Method.Name);
        }
        #endregion

        #region ByteArrays Tests
        [Fact]
        public void ByteArraysEqual_SameReference_ReturnsTrue()
        {
            //arrange
            byte[] array = [1, 2, 3];

            //act
            bool result = LinqHelpers.ByteArraysEqual(array, array);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ByteArraysEqual_EqualArrays_ReturnsTrue()
        {
            //arrange
            byte[] left = [1, 2, 3];
            byte[] right = [1, 2, 3];

            //act
            bool result = LinqHelpers.ByteArraysEqual(left, right);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ByteArraysEqual_DifferentArrays_ReturnsFalse()
        {
            //arrange
            byte[] left = [1, 2, 3];
            byte[] right = [1, 2, 4];

            //act
            bool result = LinqHelpers.ByteArraysEqual(left, right);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ByteArraysEqual_DifferentLengths_ReturnsFalse()
        {
            //arrange
            byte[] left = [1, 2, 3];
            byte[] right = [1, 2];

            //act
            bool result = LinqHelpers.ByteArraysEqual(left, right);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ByteArraysEqual_LeftNull_ReturnsFalse()
        {
            //arrange
            byte[] right = [1, 2, 3];

            //act
            bool result = LinqHelpers.ByteArraysEqual(null, right);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ByteArraysEqual_RightNull_ReturnsFalse()
        {
            //arrange
            byte[] left = [1, 2, 3];

            //act
            bool result = LinqHelpers.ByteArraysEqual(left, null);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void ByteArraysNotEqual_DifferentArrays_ReturnsTrue()
        {
            //arrange
            byte[] left = [1, 2, 3];
            byte[] right = [1, 2, 4];

            //act
            bool result = LinqHelpers.ByteArraysNotEqual(left, right);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void ByteArraysNotEqual_EqualArrays_ReturnsFalse()
        {
            //arrange
            byte[] left = [1, 2, 3];
            byte[] right = [1, 2, 3];

            //act
            bool result = LinqHelpers.ByteArraysNotEqual(left, right);

            //assert
            Assert.False(result);
        }
        #endregion

        #region CompareGuids Tests
        [Fact]
        public void CompareGuids_FirstGreater_ReturnsPositive()
        {
            //arrange
            Guid first = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
            Guid second = Guid.Parse("00000000-0000-0000-0000-000000000000");

            //act
            int result = LinqHelpers.CompareGuids(first, second);

            //assert
            Assert.True(result > 0);
        }

        [Fact]
        public void CompareGuids_SecondGreater_ReturnsNegative()
        {
            //arrange
            Guid first = Guid.Parse("00000000-0000-0000-0000-000000000000");
            Guid second = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

            //act
            int result = LinqHelpers.CompareGuids(first, second);

            //assert
            Assert.True(result < 0);
        }

        [Fact]
        public void CompareGuids_Equal_ReturnsZero()
        {
            //arrange
            Guid guid = Guid.Parse("12345678-1234-1234-1234-123456789012");

            //act
            int result = LinqHelpers.CompareGuids(guid, guid);

            //assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareGuids_BothNull_ReturnsZero()
        {
            //act
            int result = LinqHelpers.CompareGuids(null, null);

            //assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CompareGuids_FirstNull_ReturnsNegative()
        {
            //arrange
            Guid second = Guid.NewGuid();

            //act
            int result = LinqHelpers.CompareGuids(null, second);

            //assert
            Assert.True(result < 0);
        }

        [Fact]
        public void CompareGuids_SecondNull_ReturnsPositive()
        {
            //arrange
            Guid first = Guid.NewGuid();

            //act
            int result = LinqHelpers.CompareGuids(first, null);

            //assert
            Assert.True(result > 0);
        }
        #endregion

        #region DateTime Static Fields/Properties Tests
        [Fact]
        public void GetMaxDateTimOffsetField_ReturnsCorrectExpression()
        {
            //act
            var result = LinqHelpers.GetMaxDateTimOffsetField();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(DateTimeOffset), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("MaxValue", memberExpr.Member.Name);
        }

        [Fact]
        public void GetMinDateTimOffsetField_ReturnsCorrectExpression()
        {
            //act
            var result = LinqHelpers.GetMinDateTimOffsetField();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(DateTimeOffset), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("MinValue", memberExpr.Member.Name);
        }

        [Fact]
        public void GetNowDateTimOffsetProperty_ReturnsCorrectExpression()
        {
            //act
            var result = LinqHelpers.GetNowDateTimOffsetProperty();

            //assert
            Assert.IsType<MemberExpression>(result, exactMatch: false);
            Assert.Equal(typeof(DateTimeOffset), result.Type);
            var memberExpr = (MemberExpression)result;
            Assert.Equal("UtcNow", memberExpr.Member.Name);
        }
        #endregion

        #region Enum Tests
        [Fact]
        public void GetHasFlagCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var instance = Expression.Constant(System.IO.FileAttributes.ReadOnly);
            var operand = Expression.Convert(Expression.Constant(System.IO.FileAttributes.ReadOnly), typeof(System.Enum));

            //act
            var result = instance.GetHasFlagCall(operand);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("HasFlag", ((MethodCallExpression)result).Method.Name);
        }
        #endregion

        #region Additional LINQ Method Tests
        [Fact]
        public void GetAllCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());
            var param = Expression.Parameter(typeof(Product), "p");
            var predicate = Expression.Lambda<Func<Product, bool>>(
                Expression.Constant(true),
                param
            );

            //act
            var result = queryable.GetAllCall(predicate);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("All", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetContainsCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(sourceArray123.AsQueryable());
            var item = Expression.Constant(2);

            //act
            var result = queryable.GetContainsCall(item);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Contains", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetConcatCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var first = Expression.Constant(sourceArray12.AsQueryable());
            var second = Expression.Constant(sourceArray34.AsQueryable());

            //act
            var result = first.GetConcatCall(second);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Concat", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetExceptCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var first = Expression.Constant(sourceArray123.AsQueryable());
            var second = Expression.Constant(sourceArray23.AsQueryable());

            //act
            var result = first.GetExceptCall(second);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Except", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetUnionCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var first = Expression.Constant(sourceArray12.AsQueryable());
            var second = Expression.Constant(sourceArray23.AsQueryable());

            //act
            var result = first.GetUnionCall(second);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Union", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetLastCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Product() }.AsQueryable());

            //act
            var result = queryable.GetLastCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Last", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetLastOrDefaultCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetLastOrDefaultCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("LastOrDefault", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSingleCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Product() }.AsQueryable());

            //act
            var result = queryable.GetSingleCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("Single", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSingleOrDefaultCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(Enumerable.Empty<Product>().AsQueryable());

            //act
            var result = queryable.GetSingleOrDefaultCall();

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("SingleOrDefault", ((MethodCallExpression)result).Method.Name);
        }

        [Fact]
        public void GetSelectManyCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var queryable = Expression.Constant(new[] { new Category() }.AsQueryable());
            var param = Expression.Parameter(typeof(Category), "c");
            var selector = Expression.Lambda<Func<Category, IEnumerable<Product>>>(
                Expression.Property(param, "Products"),
                param
            );

            //act
            var result = queryable.GetSelectManyCall(selector);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("SelectMany", ((MethodCallExpression)result).Method.Name);
        }
        #endregion

        #region GetGuidCopareCall Tests
        [Fact]
        public void GetGuidCopareCall_ReturnsCorrectMethodCall()
        {
            //arrange
            var first = Expression.Constant(Guid.NewGuid(), typeof(Guid?));
            var second = Expression.Constant(Guid.NewGuid(), typeof(Guid?));

            //act
            var result = LinqHelpers.GetGuidCopareCall(first, second);

            //assert
            Assert.IsType<MethodCallExpression>(result, exactMatch: false);
            Assert.Equal("CompareGuids", ((MethodCallExpression)result).Method.Name);
        }
        #endregion
    }
}