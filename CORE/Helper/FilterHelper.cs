using System.Linq.Expressions;

namespace CORE.Helper;

public static class FilterHelper
{
    public static void InjectTenantFilter<TEntity>(bool useTenantIdFilter, string tenantProperyName, int? tenantId,
        ref Expression<Func<TEntity, bool>>? filter)
    {
        if (!useTenantIdFilter || tenantId is null) return;

        var parameter = Expression.Parameter(typeof(TEntity), "p");

        var left = Expression.Property(parameter, tenantProperyName);

        var right = Expression.Constant(tenantId, left.Type);

        var expression =
            (Expression<Func<TEntity, bool>>)Expression.Lambda(Expression.Equal(left, right), parameter);

        filter = filter is null ? expression : ExpressionHelper.True<TEntity>().And(filter).And(expression);
    }
}