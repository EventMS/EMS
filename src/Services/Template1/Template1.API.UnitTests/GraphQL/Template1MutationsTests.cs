using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.BuildingBlocks.EventLogEF;
using EMS.BuildingBlocks.EventLogEF.Services;
using EMS.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using SharedTesting.Helper;
using Template1.API.Context;
using Template1.API.Controllers.Request;
using Template1.API.GraphQlQueries;
using Template1.API.Mapper;
using TemplateWebHost.Customization.Settings;

namespace Template1.API.UnitTests.GraphQL
{
    [TestFixture]
    class Template1MutationsTests : BaseMutationsSetupTests<Template1Context>
    {
        
        #region Setup
        private Template1Mutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new Template1Mutations(_context, _eventService, mapper);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<Template1Profile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreateTemplate1_ValidRequest_AddedToDatabase()
        {
            var request = new CreateTemplate1Request()
            {
                Name = "Te"
            };

            await _mutations.CreateTemplate1Async(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.Template1s.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.Template1s.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<Template1CreatedEvent>());
        }

        [Test]
        public async Task CreateTemplate1_InvalidRequest_DatabaseFails()
        {
            var request = new CreateTemplate1Request()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreateTemplate1Async(request));
            await _publish.Received(0).Publish(Arg.Any<Template1CreatedEvent>());
        }


    }
}
