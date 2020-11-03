using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service_Services.API;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.EventParticipant_Services.API.GraphQlQueries;
using EMS.EventParticipant_Services.API.Mapper;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.Execution;

namespace EMS.EventParticipant_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class EventParticipantMutationsTests : BaseMutationsSetupTests<EventParticipantContext>
    {

        
        #region Setup
        private EventParticipantMutations _mutations;
        private CurrentUser _currentUser;
        

        [SetUp]
        public void SetUp()
        {
            _currentUser = new CurrentUser(Guid.NewGuid(),new List<ClubPermission>()
            {
                new ClubPermission()
                {
                    ClubId = Guid.NewGuid(),
                    SubscriptionId = Guid.NewGuid(),
                    UserRole = "Member"
                }
            });
            var mapper = CreateMapper();
            _mutations = new EventParticipantMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<EventParticipantProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task SignUpFreeEventAsync_ValidRequest_AddedToDatabase()
        {
            var e = new Event()
            {
                EventId = Guid.NewGuid(),
                ClubId = _currentUser.ClubPermissions.First().ClubId,
                PublicPrice = 50,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        Price = 0,
                        ClubSubscriptionId = _currentUser.ClubPermissions.First().SubscriptionId.Value,
                    }
                }
            };
            using (var context = _factory.CreateContext())
            {
                context.Events.Add(e);
                context.SaveChanges();
            }

            await _mutations.SignUpFreeEventAsync(e.EventId, _currentUser);

            using (var context = _factory.CreateContext())
            {
                var ep = context.EventParticipants.FirstOrDefault(ep => ep.UserId == _currentUser.UserId);
                Assert.That(ep, Is.Not.Null);
                Assert.That(context.EventParticipants.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<SignUpEventSuccessEvent>());
        }

        [Test]
        public async Task SignUpFreeEventAsync_ValidRequest_PublishedEvent()
        {
            var e = new Event()
            {
                EventId = Guid.NewGuid(),
                ClubId = _currentUser.ClubPermissions.First().ClubId,
                PublicPrice = 50,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        Price = 2,
                        ClubSubscriptionId = _currentUser.ClubPermissions.First().SubscriptionId.Value,
                    }
                }
            };
            using (var context = _factory.CreateContext())
            {
                context.Events.Add(e);
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.SignUpFreeEventAsync(e.EventId, _currentUser));
        }

        [Test]
        public async Task SignUpFreeEventAsync_ValidRequest2_PublishedEvent()
        {
            var e = new Event()
            {
                EventId = Guid.NewGuid(),
                ClubId = _currentUser.ClubPermissions.First().ClubId,
                PublicPrice = 50,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        Price = 2,
                        ClubSubscriptionId = _currentUser.ClubPermissions.First().SubscriptionId.Value,
                    }
                }
            };
            using (var context = _factory.CreateContext())
            {
                context.Events.Add(e);
                context.SaveChanges();
            }

            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.SignUpFreeEventAsync(e.EventId, _currentUser));
        }

        [Test]
        public async Task SignUpFreeEventAsync_ValidRequest3_PublishedEvent()
        {
            var e = new Event()
            {
                EventId = Guid.NewGuid(),
                ClubId = _currentUser.ClubPermissions.First().ClubId,
                PublicPrice = 50,
                EventPrices = new List<EventPrice>()
                {
                    new EventPrice()
                    {
                        Price = 2,
                        ClubSubscriptionId = _currentUser.ClubPermissions.First().SubscriptionId.Value,
                    }
                }
            };
            using (var context = _factory.CreateContext())
            {
                context.Events.Add(e);
                context.SaveChanges();
            }
            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.SignUpFreeEventAsync(e.EventId, _currentUser));
        }
    }
}
