﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.RequestModel
{
    public abstract class BaseRequestModel<TModel> where TModel : EntityBase
    {
        public string Id { get; set; }
        public List<string> Tables { get; set; }

        public string Keyword { get; set; }
        public string OrderBy { get; set; }
        public string IsAscending { get; set; }
        public OrderByRequest Request { get; }
        public int Page { get; set; }
        public int PageCount { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ParentId { get; set; }
        
        protected Expression<Func<TModel, bool>> ExpressionObj = e => true;

        protected BaseRequestModel(string keyword, string orderBy, string isAscending)
        {
            if (string.IsNullOrEmpty(keyword)) keyword = "";
            Keyword = keyword.ToLower();

            if (!string.IsNullOrWhiteSpace(orderBy)) OrderBy = orderBy;
            if (!string.IsNullOrWhiteSpace(isAscending)) IsAscending = isAscending;
            Request = new OrderByRequest
            {
                PropertyName = string.IsNullOrWhiteSpace(OrderBy) ? "Modified" : OrderBy,
                IsAscending = isAscending == "True"
            };

            Page = 1;
            PageCount = 10;
        }

        public abstract Expression<Func<TModel, bool>> GetExpression();


        public Func<IQueryable<TSource>, IOrderedQueryable<TSource>> OrderByExpression<TSource>()
        {
            var propertyName = Request.PropertyName;
            var ascending = Request.IsAscending;
            var source = Expression.Parameter(typeof(IQueryable<TSource>), "source");
            var item = Expression.Parameter(typeof(TSource), "item");
            var member = Expression.Property(item, propertyName);
            var selector = Expression.Quote(Expression.Lambda(member, item));
            var body = Expression.Call(
                typeof(Queryable), ascending ? "OrderBy" : "OrderByDescending",
                new[] { item.Type, member.Type },
                source, selector);
            var lambda = Expression.Lambda<Func<IQueryable<TSource>, IOrderedQueryable<TSource>>>(body, source);
            var expression = lambda.Compile();
            return expression;
        }

        public IQueryable<TModel> GetSkipAndTakeQuery(IQueryable<TModel> queryable)
        {
            if (Page != -1)
            {
                queryable = queryable.Skip((Page - 1) * PageCount).Take(PageCount);
            }

            return queryable;
        }

        public IQueryable<TModel> GetDataQuery(IQueryable<TModel> queryable)
        {
            return queryable.Where(GetExpression());
        }

        public TModel GetFirstDataQuery(IQueryable<TModel> queryable)
        {
            return queryable.First(GetExpression());
        }

        public IQueryable<TModel> GetOrderedDataQuery(IQueryable<TModel> queryable)
        {
            queryable = queryable.Where(GetExpression());
            queryable = OrderByExpression<TModel>()(queryable);
            return queryable;
        }


        public class OrderByRequest
        {
            public string PropertyName { get; set; }
            public bool IsAscending { get; set; }
        }
    }





    public static class ExpressionHelper
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }
    }


    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }


    public class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _newValue;
        private readonly Expression _oldValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
}
