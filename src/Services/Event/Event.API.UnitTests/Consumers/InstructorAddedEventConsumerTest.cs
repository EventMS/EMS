using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context.Model;
using EMS.Event_Services.API.Events;
using EMS.Events;
using EMS.SharedTesting.Factories;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace EMS.Event_Services.API.UnitTests.Consumers
{
    [TestFixture]
    class InstructorAddedEventConsumerTest : EventConsumerTest<InstructorAddedEventConsumer>
    {
        [SetUp]
        public void SetUp()
        {
            var content = _factory.CreateContext();
            _consumer = new InstructorAddedEventConsumer(content, CreateMapper());
            _harness.Start().Wait();
        }

        [TearDown]
        public void TearDown()
        {
            _harness.Stop().Wait();
        }

        [Test]
        public async Task Consume_ClubDoesNotExist_DoesNotCreate()
        {
            var @event = new InstructorAddedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Instructors.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public async Task Consume_InstructorDoesNotExist_Creates()
        {
            var @event = new InstructorAddedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.SaveChanges();
            }

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Instructors.Count(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task Consume_InstructorDoesExist_DoesNotCreateAnother()
        {
            var @event = new InstructorAddedEvent()
            {
                ClubId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(new Club()
                {
                    ClubId = @event.ClubId
                });
                context.Instructors.Add(new Instructor()
                {
                    ClubId = @event.ClubId,
                    InstructorId = @event.UserId
                });
                context.SaveChanges();
            }

            await SendEvent(@event);


            using (var context = _factory.CreateContext())
            {
                Assert.That(context.Instructors.Count(), Is.EqualTo(1));
            }
        }
    }
}