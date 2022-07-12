using System.Linq.Expressions;

namespace ProEShop.Common.Helpers;

public static class ExpressionHelpers
{
    public static Expression<Func<T, bool>> CreateExpression<T>(string propertyName, object propertyValue)
    {
        // full Expression => x => x.Title == "Value"
        var parameter = Expression.Parameter(typeof(T)); //Generate x
        var property = Expression.Property(parameter, propertyName); // x.Title
        if (propertyValue is string)
            propertyValue = propertyValue.ToString()?.Trim();
        var constantValue = Expression.Constant(propertyValue); // "Value"
        var equal = Expression.Equal(property, constantValue); // x.Title == "Value"
        return Expression.Lambda<Func<T, bool>>(equal, parameter); // x => x.Title == "Value"
    }
    public static IOrderedQueryable<T> CreateOrderByExpression<T>(this IQueryable<T> query,string propertyName,string isAsc)
    {
        // x=> x.Id
        var parameter = Expression.Parameter(typeof(T));
        var conversion = Expression.Convert(Expression.Property(parameter, propertyName), typeof(object));
        var exp = Expression.Lambda<Func<T, object>>(conversion, parameter);

        IOrderedQueryable<T> result = null;
        result = isAsc == "Asc" ? query.OrderBy(exp) : query.OrderByDescending(exp);
        return result;
    }
}
