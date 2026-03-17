namespace LogicBuilder.Expressions.Utils.ExpressionBuilder.Operand
{
    public class ConstantContainer<T> : ConstantContainer
    {
        public ConstantContainer(T typedProperty) : base(typedProperty)
        {
            TypedProperty = typedProperty;
        }

        public T? TypedProperty { get => (T?)Property; set => Property = value; }
    }

    abstract public class ConstantContainer(object? property)
    {
        public object? Property { get; set; } = property;
    }
}
