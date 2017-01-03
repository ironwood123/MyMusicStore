using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Models.Utilities
{
    public static class ExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static Expression<Func<TEntity, bool>> GetWhereExpression<TEntity>(IList<Filter> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(TEntity), "t");
            Expression tempexp = null;

            if (filters.Count == 1)
                tempexp = GetExpression<TEntity>(param, filters[0]);
            else if (filters.Count == 2)
                tempexp = GetExpression<TEntity>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (tempexp == null)
                        tempexp = GetExpression<TEntity>(param, filters[0], filters[1]);
                    else
                        tempexp = Expression.AndAlso(tempexp, GetExpression<TEntity>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        tempexp = Expression.AndAlso(tempexp, GetExpression<TEntity>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<TEntity, bool>>(tempexp, param);
        }

        public static Expression<Func<TEntity, bool>> GetWhereExpression<TEntity>(KendoFilters filter)
        {
            //remove empty value filter
            for (int i = 0; i < filter.filters.Count;)
            {
                var f = filter.filters[i];
                if (string.IsNullOrEmpty(f.value))
                {
                    filter.filters.Remove(f);
                }
                else
                    i++;
                if (filter.filters.Count == 0)
                    break;
            }

            if (filter != null && (filter.filters != null && filter.filters.Count > 0))
            {
                ParameterExpression param = Expression.Parameter(typeof(TEntity), "t");
                Expression tempexp = null;

                if (filter.filters.Count == 1)
                    tempexp = GetExpression<TEntity>(param, filter.filters[0]);
                else if (filter.filters.Count == 2)
                    tempexp = GetExpression<TEntity>(param, filter.filters[0], filter.filters[1]);
                else
                {
                    while (filter.filters.Count > 0)
                    {
                        var f1 = filter.filters[0];
                        var f2 = filter.filters[1];

                        if (tempexp == null)
                            tempexp = GetExpression<TEntity>(param, filter.filters[0], filter.filters[1]);
                        else
                            tempexp = Expression.AndAlso(tempexp, GetExpression<TEntity>(param, filter.filters[0], filter.filters[1]));

                        filter.filters.Remove(f1);
                        filter.filters.Remove(f2);

                        if (filter.filters.Count == 1)
                        {
                            tempexp = Expression.AndAlso(tempexp, GetExpression<TEntity>(param, filter.filters[0]));
                            filter.filters.RemoveAt(0);
                        }
                    }
                }

                return Expression.Lambda<Func<TEntity, bool>>(tempexp, param);



            }
            else
                return null;




        }

        private static Expression GetExpression<TEntity>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Op.isFalse:
                    return Expression.NotEqual(Expression.Convert(member, typeof(bool)), constant);
                case Op.isTrue:
                    return Expression.Equal(Expression.Convert(member, typeof(bool)), constant);
                case Op.Equals:
                    return Expression.Equal(member, constant);
                case Op.NotEqual:
                    return Expression.NotEqual(member, constant);

                case Op.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Op.LessThan:
                    return Expression.LessThan(member, constant);

                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Op.Contains:
                    return Expression.Call(member, containsMethod, constant);

                case Op.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Op.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
                case Op.Not:
                    return Expression.Not(member);

            }

            return null;
        }

        private static Expression GetExpression<TEntity>(ParameterExpression param, KendoFilter filter)
        {


            string[] props = filter.field.Split('.');
            //IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);

            Expression expr = param;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }


            ConstantExpression constant = Expression.Constant(filter.value);


            switch (filter.Operator)
            {
                case "isfalse":
                    return Expression.NotEqual(Expression.Convert(expr, typeof(bool)), constant);
                case "istrue":
                    return Expression.Equal(Expression.Convert(expr, typeof(bool)), constant);
                case "eq":
                    return Expression.Equal(expr, constant);
                case "neq":
                    return Expression.NotEqual(expr, constant);

                case "gt":
                    return Expression.GreaterThan(expr, constant);

                case "gte":
                    return Expression.GreaterThanOrEqual(expr, constant);

                case "lt":
                    return Expression.LessThan(expr, constant);

                case "lte":
                    return Expression.LessThanOrEqual(expr, constant);

                case "contains":
                    return Expression.Call(expr, containsMethod, constant);

                case "startswith":
                    return Expression.Call(expr, startsWithMethod, constant);

                case "endswith":
                    return Expression.Call(expr, endsWithMethod, constant);
                case "not":
                    return Expression.Not(expr);

            }

            return null;
        }

        private static Expression GetExpression<TEntity>(ParameterExpression param, string filterField, string filterValue, string filterOperator)
        {

            MemberExpression member = Expression.Property(param, filterField);
            ConstantExpression constant = Expression.Constant(filterValue);


            switch (filterOperator)
            {
                case "isfalse":
                    return Expression.NotEqual(Expression.Convert(member, typeof(bool)), constant);
                case "istrue":
                    return Expression.Equal(Expression.Convert(member, typeof(bool)), constant);
                case "eq":
                    return Expression.Equal(member, constant);
                case "neq":
                    return Expression.NotEqual(member, constant);

                case "gt":
                    return Expression.GreaterThan(member, constant);

                case "gte":
                    return Expression.GreaterThanOrEqual(member, constant);

                case "lt":
                    return Expression.LessThan(member, constant);

                case "lte":
                    return Expression.LessThanOrEqual(member, constant);

                case "contains":
                    return Expression.Call(member, containsMethod, constant);

                case "startswith":
                    return Expression.Call(member, startsWithMethod, constant);

                case "endswith":
                    return Expression.Call(member, endsWithMethod, constant);
                case "not":
                    return Expression.Not(member);

            }

            return null;
        }


        private static BinaryExpression GetExpression<TEntity>
        (ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<TEntity>(param, filter1);
            Expression bin2 = GetExpression<TEntity>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

        private static BinaryExpression GetExpression<TEntity>
        (ParameterExpression param, KendoFilter filter1, KendoFilter filter2)
        {
            Expression bin1 = GetExpression<TEntity>(param, filter1);
            Expression bin2 = GetExpression<TEntity>(param, filter2);
            return Expression.AndAlso(bin1, bin2);

        }
        public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy<TEntity>(string orderColumn, string orderType)
        {
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
            }

            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
        }



        public static string BuildWhereClause<T>(int index, string logic,
         KendoFilter filter, List<object> parameters)
        {
            var entityType = (typeof(T));
            var property = entityType.GetProperty(filter.field);

            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                    if (typeof(DateTime).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(DateTime.Parse(filter.value).Date);
                        return string.Format("EntityFunctions.TruncateTime({0}){1}@{2}",
                            filter.field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    if (typeof(int).IsAssignableFrom(property.PropertyType))
                    {
                        parameters.Add(int.Parse(filter.value));
                        return string.Format("{0}{1}@{2}",
                            filter.field,
                            ToLinqOperator(filter.Operator),
                            index);
                    }
                    parameters.Add(filter.value);
                    return string.Format("{0}{1}@{2}",
                        filter.field,
                        ToLinqOperator(filter.Operator),
                        index);
                case "startswith":
                    parameters.Add(filter.value);
                    return string.Format("{0}.StartsWith(" + "@{1})",
                        filter.field,
                        index);
                case "endswith":
                    parameters.Add(filter.value);
                    return string.Format("{0}.EndsWith(" + "@{1})",
                        filter.field,
                        index);
                case "contains":
                    parameters.Add(filter.value);
                    return string.Format("{0}.Contains(" + "@{1})",
                        filter.field,
                        index);
                default:
                    throw new ArgumentException(
                        "This operator is not yet supported for this Grid",
                        filter.Operator);
            }
        }

        public static string ToLinqOperator(string @operator)
        {
            switch (@operator.ToLower())
            {
                case "eq": return " == ";
                case "neq": return " != ";
                case "gte": return " >= ";
                case "gt": return " > ";
                case "lte": return " >= ";
                case "lt": return " > ";
                case "or": return " || ";
                case "and": return " && ";
                default: return null;
            }
        }


    }
}
