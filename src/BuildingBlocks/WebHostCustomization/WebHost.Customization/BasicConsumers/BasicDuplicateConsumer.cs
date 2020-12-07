using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TemplateWebHost.Customization.BasicConsumers
{
    /// <summary>
    /// Extension for ease of finding Entity framework primary keys. 
    /// Based on: https://stackoverflow.com/questions/30688909/how-to-get-primary-key-value-with-entity-framework-core
    /// </summary>
    public static class Extensions
    {
        private static IEnumerable<string> FindPrimaryKeyNames<T>(this DbContext dbContext, T entity)
        {
            return from p in dbContext.FindPrimaryKeyProperties(entity)
                select p.Name;
        }

        public static object[] FindPrimaryKeyValues<T>(this DbContext dbContext, T entity)
        {
            return dbContext.FindPrimaryKeyProperties(entity)
                .Select(p => entity.GetPropertyValue(p.Name))
                .ToArray();
        }

        private static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T entity)
        {
            return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
        }

        private static object GetPropertyValue<T>(this T entity, string name)
        {
            return entity.GetType().GetProperty(name).GetValue(entity, null);
        }

    }
    /// <summary>
    /// Simple duplicating consumer, that based on Automapper configuration creates a entry in DB. 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public class BasicDuplicateConsumer<TContext, TType, TEvent> :
        IConsumer<TEvent> where TEvent : class where TType : class where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly IMapper _mapper;

        public BasicDuplicateConsumer(TContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual Task AddAction(TType entity)
        {
            return Task.CompletedTask;
        }

        public virtual async Task<TType> FindEntity(TType entity, TEvent e)
        {
            return await _context.Set<TType>().FindAsync(_context.FindPrimaryKeyValues(entity).ToArray());
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var entity = _mapper.Map<TType>(context.Message);
            var alreadyFound = await FindEntity(entity, context.Message);
            if (alreadyFound == null)
            {
                await _context.AddAsync(entity);
                await AddAction(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
