using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.Context.Model;
using EMS.EventVerification_Services.API.Controllers.Request;
using EMS.EventVerification_Services.API.GraphQlQueries;
using EMS.EventVerification_Services.API.Mapper;
using HotChocolate.Execution;

namespace EMS.EventVerification_Services.API.UnitTests.GraphQL
{
    
    [TestFixture]
    class EventVerificationMutationsTests : BaseMutationsSetupTests<EventVerificationContext>
    {
        
        #region Setup
        private EventVerificationMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new EventVerificationMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventVerificationProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task VerifyCodeAsync_CodeDoesNotExist()
        {
            var request = new VerifyCodeAsyncRequest()
            {
                EventId = Guid.NewGuid(),
                Code = "0001"
            };

            Assert.ThrowsAsync<QueryException>(async()=> await _mutations.VerifyCodeAsync(request));
        }

        [Test]
        public async Task VerifyCodeAsync_EventDoesNotExist()
        {
            var request = new VerifyCodeAsyncRequest()
            {
                EventId = Guid.NewGuid(),
                Code = "0001"
            };

            using (var context = _factory.CreateContext())
            {
                context.EventVerifications.Add(new EventVerification()
                {
                    EventId = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                });
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () => await _mutations.VerifyCodeAsync(request));
        }

        [Test]
        public async Task VerifyCodeAsync_EventDoesNotExist_AlreadyVerified()
        {
            var request = new VerifyCodeAsyncRequest()
            {
                EventId = Guid.NewGuid(),
                Code = "0001"
            };

            using (var context = _factory.CreateContext())
            {
                context.EventVerifications.Add(new EventVerification()
                {
                    EventId = request.EventId,
                    UserId = Guid.NewGuid(),
                    Status = PresenceStatusEnum.Attend
                });
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () => await _mutations.VerifyCodeAsync(request));
        }

        [Test]
        public async Task VerifyCodeAsync_DoesExist_ChangesToAttended()
        {
            var request = new VerifyCodeAsyncRequest()
            {
                EventId = Guid.NewGuid(),
                Code = "0001"
            };

            using (var context = _factory.CreateContext())
            {
                context.EventVerifications.Add(new EventVerification()
                {
                    EventId = request.EventId,
                    UserId = Guid.NewGuid()
                });
                context.SaveChanges();
            }

            await _mutations.VerifyCodeAsync(request);

            using (var context = _factory.CreateContext())
            {
                Assert.That(context.EventVerifications.Count(), Is.EqualTo(1));
                var ev = context.EventVerifications.SingleOrDefault(ev => ev.EventId == request.EventId);

                Assert.That(ev, Is.Not.Null);
                Assert.That(ev.Status, Is.EqualTo(PresenceStatusEnum.Attend));
            }
        }
    }
}
