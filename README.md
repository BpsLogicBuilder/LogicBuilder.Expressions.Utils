# LogicBuilder.Expressions.Utils

[![CI](https://github.com/BpsLogicBuilder/LogicBuilder.Expressions.Utils/actions/workflows/ci.yml/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.Expressions.Utils/actions/workflows/ci.yml)
[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.Expressions.Utils/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.Expressions.Utils/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.Expressions.Utils/graph/badge.svg?token=FEZ6S92MQL)](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.Expressions.Utils)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.Expressions.Utils&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.Expressions.Utils)
[![NuGet](https://img.shields.io/nuget/v/LogicBuilder.Expressions.Utils.svg)](https://www.nuget.org/packages/LogicBuilder.Expressions.Utils)

A powerful runtime expression builder that transforms metadata descriptors from **LogicBuilder.Structures** into executable LINQ Expression Trees. This library bridges the gap between serializable query metadata and strongly-typed, compiled LINQ expressions.

## Purpose

LogicBuilder.Expressions.Utils provides the **execution engine** for the LogicBuilder framework. It takes descriptor metadata (POCOs from LogicBuilder.Structures) and generates fully functional LINQ expressions through a two-stage transformation:

1. **Descriptors → Operators** (via AutoMapper)
2. **Operators → Expression Trees** (via `Build()` methods)

This architecture enables:
- **Separation of concerns**: Metadata structure (descriptors) separate from expression building logic (operators)
- **Serializability**: Store query logic as JSON/XML, then materialize as compiled LINQ at runtime
- **Type safety**: Generate strongly-typed expressions from loosely-typed metadata
- **Testability**: Test expression logic using both descriptor and operator layers

## Architecture

### Two-Layer Design

#### Layer 1: Expression Operators (`IExpressionPart`)
Stateful objects that build LINQ Expression Trees:
- **Binary Operators**: `EqualsBinaryOperator`, `GreaterThanBinaryOperator`, `AndBinaryOperator`, etc.
- **Unary Operators**: `NotOperator`, `ConvertOperator`, `CastOperator`, etc.
- **Method Call Operators**: `ContainsOperator`, `StartsWithOperator`, `AnyOperator`, `AllOperator`, etc.
- **Queryable Operators**: `WhereOperator`, `SelectOperator`, `OrderByOperator`, `GroupByOperator`, etc.
- **Operand Operators**: `ParameterOperator`, `ConstantOperator`, `MemberSelectorOperator`, etc.

Each operator implements `IExpressionPart` with a `Build()` method that returns a `System.Linq.Expressions.Expression`.

#### Layer 2: AutoMapper Profiles
Transform descriptors to operators:
- An AutoMapper profile is useful for mapping descriptor classes to operator classes
- Handles parameter scope management via mapping context
- Preserves expression structure during transformation

e.g.:
```c#
        public void BuildFilter()
        {
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
            });
            IMapper mapper = config.CreateMapper();

            Expression<Func<Product, bool>> filter = GetFilterExpression<Product>
            (
                //$it => $it.AlternateAddresses.Any(address => (address.City == "Redmond"))
                new FilterLambdaDescriptor
                (
                    new AnyDescriptor
                    (
                        new MemberSelectorDescriptor("AlternateAddresses", new ParameterDescriptor("$it")),
                        new EqualsBinaryDescriptor
                        (
                            new MemberSelectorDescriptor("City", new ParameterDescriptor("address")),
                            new ConstantDescriptor("Redmond")
                        ),
                        "address"
                    ),
                    typeof(Product),
                    "$it"
                )
            );

            Expression<Func<T, bool>> GetFilterExpression<T>(FilterLambdaDescriptor descriptor)
                => (Expression<Func<T, bool>>)mapper.Map<FilterLambdaOperator>
                (
                    descriptor,
                    opts => opts.Items["parameters"] = new Dictionary<string, ParameterExpression>()
                ).Build();
        }

        public void BuildSelector()
        {
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ExpressionOperatorsMappingProfile>();
            });
            IMapper mapper = config.CreateMapper();

            Expression<Func<IQueryable<Category>, Category>> selector = GetSelectorExpression<IQueryable<Category>, Category>
            (
                //$it => $it.FirstOrDefault(a => (a.CategoryID == -1))
                new SelectorLambdaDescriptor
                (
                    new FirstOrDefaultDescriptor
                    (
                        new ParameterDescriptor("$it"),
                        new EqualsBinaryDescriptor
                        (
                            new MemberSelectorDescriptor("CategoryID", new ParameterDescriptor("a")),
                            new ConstantDescriptor(-1)
                        ),
                        "a"
                    ),
                    typeof(IQueryable<Category>),
                    typeof(Category),
                    "$it"
                )
            );
            
            Expression<Func<T, TResult>> GetSelectorExpression<T, TResult>(SelectorLambdaDescriptor descriptor)
                => (Expression<Func<T, TResult>>)mapper.Map<SelectorLambdaOperator>
                (
                    descriptor,
                    opts => opts.Items["parameters"] = new Dictionary<string, ParameterExpression>()
                ).Build();
        }
```
`ExpressionOperatorsMappingProfile` is a mapping profile from `LogicBuilder.EntityFrameworkCore.SqlServer`. The descriptor classes are fully serializable in version 8.0.0.

## Requirements

- **.NET Standard 2.0** or higher
- **AutoMapper** for descriptor-to-operator mapping
- **LogicBuilder.Structures** for descriptor metadata classes

## Related Libraries

- **LogicBuilder.Structures**: Provides descriptor metadata classes (POCOs)
- **LogicBuilder.EntityFrameworkCore.SqlServer**: EF Core integration and extensive test coverage
- **LogicBuilder.Expressions.Utils.Expansions**: Additional expression manipulation utilities
