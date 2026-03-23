using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogicBuilder.Expressions.Utils.Expansions
{
    public static class ExpansionsHelper
    {
        public static IEnumerable<Expression<Func<TSource, object>>> GetExpansionSelectors<TSource>(this SelectExpandDefinition selectExpandDefinition) where TSource : class
        {
            if (selectExpandDefinition == null)
                return [];

            return selectExpandDefinition.GetExpansions
            (
                typeof(TSource)
            )
            .Select(list => new List<Expansion>(list))
            .BuildIncludes<TSource>
            (
                selectExpandDefinition.Selects
            );
        }

        public static List<List<ExpansionOptions>> GetExpansions(this SelectExpandDefinition selectExpandDefinition, Type sourceType)
        {
            if (selectExpandDefinition == null)
                return [];

            return selectExpandDefinition.ExpandedItems.GetExpansions
            (
                sourceType
            );
        }

        private static List<List<ExpansionOptions>> GetExpansions(this IEnumerable<SelectExpandItem> selectExpandItems, Type sourceType)
        {
            return selectExpandItems.Aggregate(new List<List<ExpansionOptions>>(), (listOfExpansionLists, next) =>
            {
                Type currentParentType = sourceType.GetCurrentType();
                Type memberType = currentParentType.GetMemberInfo(next.MemberName).GetMemberType();
                Type elementType = memberType.GetCurrentType();

                ExpansionOptions expansionOption = new
                (
                    next.MemberName,
                    memberType,
                    currentParentType,
                    next.Selects,
                    GetQuery(next, memberType),
                    GetFilter(next, memberType)
                );

                List<List<ExpansionOptions>> navigationItems = [.. next.ExpandedItems.GetExpansions
                    (
                        elementType
                    )
                    .Select
                    (
                        expansions =>
                        {
                            expansions.Insert(0, expansionOption);
                            return expansions;
                        }
                    )];

                if (navigationItems.Any())
                    listOfExpansionLists.AddRange(navigationItems);
                else
                    listOfExpansionLists.Add([expansionOption]);

                return listOfExpansionLists;

                static ExpansionFilterOption? GetFilter(SelectExpandItem item, Type itemType)
                    => HasFilter(item, itemType)
                        ? new ExpansionFilterOption(item.Filter!.FilterLambdaOperator)//next.Filter not null if HasFilter
                        : null;

                static ExpansionQueryOption? GetQuery(SelectExpandItem item, Type itemType)
                    => HasQuery(item, itemType)
                        ? new ExpansionQueryOption(item.QueryFunction!.SortCollection)//next.QueryFunction not null if HasQuery
                        : null;

                static bool HasFilter(SelectExpandItem item, Type itemType)
                    => itemType.IsList() && item.Filter?.FilterLambdaOperator != null;

                static bool HasQuery(SelectExpandItem item, Type itemType)
                    => itemType.IsList() && item?.QueryFunction?.SortCollection != null;
            });
        }

        public static ICollection<Expression<Func<TSource, object>>> BuildIncludes<TSource>(this IEnumerable<List<Expansion>> includes, List<string> selects)
            where TSource : class
        {
            return GetAllExpansions([]);

            List<Expression<Func<TSource, object>>> GetAllExpansions(List<LambdaExpression> valueMemberSelectors)
            {
                string parameterName = "i";
                ParameterExpression param = Expression.Parameter(typeof(TSource), parameterName);

                valueMemberSelectors.AddSelectors(selects, param, param);

                return
                [
                    .. includes.Select(include => BuildSelectorExpression<TSource>(include, valueMemberSelectors, parameterName)),
                    .. valueMemberSelectors.Select(selector => (Expression<Func<TSource, object>>)selector),
                ];
            }
        }

        private static Expression<Func<TSource, object>> BuildSelectorExpression<TSource>(List<Expansion> fullName, List<LambdaExpression> valueMemberSelectors, string parameterName = "i")
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), parameterName);

            return (Expression<Func<TSource, object>>)Expression.Lambda
            (
                typeof(Func<,>).MakeGenericType(param.Type, typeof(object)),
                BuildSelectorExpression(param, fullName, valueMemberSelectors, parameterName),
                param
            );
        }

        //e.g. /opstenant?$top=5&$expand=Buildings($expand=Builder($expand=City))
        private static Expression BuildSelectorExpression(Expression sourceExpression, List<Expansion> parts, List<LambdaExpression> valueMemberSelectors, string parameterName = "i")
        {
            Expression parent = sourceExpression;

            //Arguments to create a nested expression when the parent expansion is a collection
            //See AddChildSeelctors() below
            List<LambdaExpression> childValueMemberSelectors = [];

            for (int i = 0; i < parts.Count; i++)
            {
                if (parent.Type.IsList())
                {
                    Expression selectExpression = GetSelectExpression
                    (
                        parts.Skip(i),
                        parent,
                        childValueMemberSelectors,
                        parameterName
                    );

                    AddChildSeelctors();

                    return selectExpression;
                }
                else
                {
                    parent = Expression.MakeMemberAccess(parent, parent.Type.GetMemberInfo(parts[i].MemberName));

                    if (parent.Type.IsList())
                    {
                        ParameterExpression childParam = Expression.Parameter(parent.GetUnderlyingElementType(), parameterName.ChildParameterName());
                        //selectors from an underlying list element must be added here.
                        childValueMemberSelectors.AddSelectors
                        (
                            parts[i].Selects,
                            childParam,
                            childParam
                        );
                    }
                    else
                    {
                        valueMemberSelectors.AddSelectors(parts[i].Selects, Expression.Parameter(sourceExpression.Type, parameterName), parent);
                    }
                }
            }

            AddChildSeelctors();

            return parent;

            //Adding childValueMemberSelectors created above and in a the recursive call:
            //i0 => i0.Builder.Name becomes
            //i => i.Buildings.Select(i0 => i0.Builder.Name)
            void AddChildSeelctors()
            {
                childValueMemberSelectors.ForEach(selector =>
                {
                    valueMemberSelectors.Add(Expression.Lambda
                    (
                        typeof(Func<,>).MakeGenericType(sourceExpression.Type, typeof(object)),
                        Expression.Call
                        (
                            typeof(Enumerable),
                            "Select",
                            [parent.GetUnderlyingElementType(), typeof(object)],
                            parent,
                            selector
                        ),
                        Expression.Parameter(sourceExpression.Type, parameterName)
                    ));
                });
            }
        }

        private static void AddSelectors(this List<LambdaExpression> valueMemberSelectors, List<string> selects, ParameterExpression param, Expression parentBody)
        {
            if (parentBody.Type.IsList() || parentBody.Type.IsLiteralType())
                return;

            valueMemberSelectors.AddRange
            (
                parentBody.Type
                    .GetSelectedMembers(selects)
                    .Select(member => Expression.MakeMemberAccess(parentBody, member))
                    .Select
                    (
                        selector => selector.Type.IsValueType
                            ? (Expression)Expression.Convert(selector, typeof(object))
                            : selector
                    )
                    .Select
                    (
                        selector => Expression.Lambda
                        (
                            typeof(Func<,>).MakeGenericType(param.Type, typeof(object)),
                            selector,
                            param
                        )
                    )
            );
        }



        private static Expression GetSelectExpression(IEnumerable<Expansion> expansions, Expression parent, List<LambdaExpression> valueMemberSelectors, string parameterName)
        {
            ParameterExpression parameter = Expression.Parameter(parent.GetUnderlyingElementType(), parameterName.ChildParameterName());
            Expression selectorBody = BuildSelectorExpression(parameter, [.. expansions], valueMemberSelectors, parameter.Name);
            return Expression.Call
            (
                typeof(Enumerable),
                "Select",
                [parameter.Type, selectorBody.Type],
                parent,
                Expression.Lambda
                (
                    typeof(Func<,>).MakeGenericType(parameter.Type, selectorBody.Type),
                    selectorBody,
                    parameter
                )
            );
        }
    }
}
