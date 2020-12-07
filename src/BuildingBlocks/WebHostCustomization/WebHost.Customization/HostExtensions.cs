using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotChocolate;
using HotChocolate.Execution;

namespace EMS.TemplateWebHost.Customization
{
    /// <summary>
    /// Usefull extensions for all services
    /// </summary>
    public static class MyExtensions
    {
        public static IMappingExpression<TSource, TDestination> Transform<TSource, TDestination, TSourceMember>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector,
            Expression<Func<TSource, TSourceMember>> transform
        )
        {
            map.ForMember(selector, config =>
                config.MapFrom(transform));
            return map;
        }

        public static async Task<T> FindOrThrowAsync<T>(this DbSet<T> set, params object[] keyValues) where T : class
        {
            var result = await set.FindAsync(keyValues);
            if (result == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown for type: " + typeof(T))
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            return result;
        }

        public static async Task<TSource> SingleOrThrowAsync<TSource>(
            [NotNull] this IQueryable<TSource> source,
            [NotNull] Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            var result = await source.SingleOrDefaultAsync(predicate, cancellationToken: cancellationToken);
            if (result == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            return result;
        }
    }
}
