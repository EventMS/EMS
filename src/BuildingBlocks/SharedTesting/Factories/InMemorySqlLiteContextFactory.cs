using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection.Emit;
using EMS.BuildingBlocks.EventLogEF;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EMS.SharedTesting.Factories
{
    //Reference to: https://www.meziantou.net/2017/09/11/testing-ef-core-in-memory-using-sqlite
    public class InMemorySqlLiteContextFactory<TContext>  : IDisposable where TContext : DbContext
    {
        private DbConnection _connection;

        private Func<DbContextOptions<TContext>, TContext> _TContextCreater;

        public InMemorySqlLiteContextFactory(Func<DbContextOptions<TContext>, TContext> tContextCreater)
        {
            _TContextCreater = tContextCreater;
        }

        private DbContextOptions<T> CreateOptions<T>() where T:DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseSqlite(_connection).Options;
        }

        public TContext CreateContext(bool shouldCreateEventLog = false)
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();
                _TContextCreater(CreateOptions<TContext>());
                using (var context = _TContextCreater(CreateOptions<TContext>()))
                {
                    context.Database.EnsureCreated();
                }

                if (shouldCreateEventLog)
                {
                    CreateEventLogContext();
                }
            }

            return _TContextCreater(CreateOptions<TContext>());
        }

        private void CreateEventLogContext()
        {
            using (var context = new EventLogContext(CreateOptions<EventLogContext>()))
            {
                context.Database.Migrate();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }


    }
}