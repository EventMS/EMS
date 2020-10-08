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

    public class BasicDuplicateConsumer<TContext, TType, TEvent> :
        IConsumer<TEvent> where TEvent : class where TType : class where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IMapper _mapper;

        public BasicDuplicateConsumer(TContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var mappedObject = _mapper.Map<TType>(context.Message);
            var alreadyFound = _context.Set<TType>().Find(_context.FindPrimaryKeyValues(mappedObject).ToArray());
            if (alreadyFound == null)
            {
                await _context.AddAsync(mappedObject);
                await _context.SaveChangesAsync();
            }
        }
    }
}
