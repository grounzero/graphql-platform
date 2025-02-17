using System.Linq.Expressions;
using System.Reflection;
using HotChocolate.Internal;
using static HotChocolate.Resolvers.Expressions.Parameters.ParameterExpressionBuilderHelpers;

#nullable enable

namespace HotChocolate.Resolvers.Expressions.Parameters;

internal sealed class SchemaParameterExpressionBuilder
    : IParameterExpressionBuilder
    , IParameterBindingFactory
    , IParameterBinding
{
    private static readonly PropertyInfo _schema =
        PureContextType.GetProperty(nameof(IPureResolverContext.Schema))!;

    public ArgumentKind Kind => ArgumentKind.Schema;

    public bool IsPure => true;

    public bool IsDefaultHandler => false;

    public bool CanHandle(ParameterInfo parameter)
        => typeof(ISchema) == parameter.ParameterType ||
           typeof(Schema) == parameter.ParameterType;

    public Expression Build(ParameterExpressionBuilderContext context)
        => Expression.Convert(
            Expression.Property(context.ResolverContext, _schema),
            context.Parameter.ParameterType);

    public IParameterBinding Create(ParameterBindingContext context)
        => this;

    public T Execute<T>(IResolverContext context)
        => (T)(object)context.Schema;

    public T Execute<T>(IPureResolverContext context)
        => (T)(object)context.Schema;
}
