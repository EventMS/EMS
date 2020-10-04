


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.API;
using Club.API.Context;
using Club.API.Controllers.Request;
using Club.API.GraphQlQueries;
using Club.API.Mapper;
using EMS.Events;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Polly;
using SharedTesting.Helper;

namespace Subscription.API.UnitTests.GraphQL
{
    [TestFixture]
    class ClubMutationsTests : BaseMutationsSetupTests<ClubContext>
    {
        
        #region Setup
        private ClubMutations _mutations;
        private CurrentUser _currentUser = new CurrentUser(Guid.NewGuid());
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = CreateMapper();
            _mutations = new ClubMutations(_context, _eventService, _mapper);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ClubProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion

        private CreateClubRequest CreateClubRequest()
        {
            return new CreateClubRequest()
            {
                Name = "Request",
                AccountNumber = "12345678",
                Address = "Adresse",
                Description = "Beskrivelse",
                PhoneNumber = "12345678",
                RegistrationNumber = "1234",
                Locations = new List<string>()
                {
                    "Room 1",
                    "Room2"
                }
            };
        }

        private UpdateClubRequest UpdateClubRequest()
        {
            return new UpdateClubRequest()
            {
                Name = "Update2",
                AccountNumber = "12345678",
                Address = "Adresse",
                Description = "Beskrivelse",
                PhoneNumber = "12345678",
                RegistrationNumber = "1234",
            };
        }


        [Test]
        public async Task CreateClub_ValidRequest_AddedToDatabase()
        {
            //Act
            var request = CreateClubRequest();
            await _mutations.CreateClubAsync(request, _currentUser);

            //Assert
            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.Name == request.Name);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubCreatedEvent>());
        }

        [Test]
        public async Task CreateClub_NameUsed_Fails()
        {
            var request = CreateClubRequest();

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(_mapper.Map<Club.API.Context.Model.Club>(request));
                context.SaveChanges();
            }
            //Act Assert
            Assert.ThrowsAsync<DbUpdateException>(async () =>
                await _mutations.CreateClubAsync(request, _currentUser));
        }

        [Test]
        public async Task CreateClub_TwoClubsWithDifferentNames_Succeds()
        {
            var request = CreateClubRequest();

            using (var context = _factory.CreateContext())
            {
                context.Clubs.Add(_mapper.Map<Club.API.Context.Model.Club>(request));
                context.SaveChanges();
            }
            //Act Assert
            request.Name = "Club2Name";
            await _mutations.CreateClubAsync(request, _currentUser);
            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.Name == request.Name);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.Clubs.Count(), Is.EqualTo(2));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubCreatedEvent>());
        }


        [Test]
        public async Task UpdateClub_ClubIsCreated_DatabaseUpdates()
        {
            var request = UpdateClubRequest();
            var clubCreated = _mapper.Map<Club.API.Context.Model.Club>(CreateClubRequest());
            using (var context = _factory.CreateContext())
            {
                
                context.Clubs.Add(clubCreated);
                context.SaveChanges();
            }


            await _mutations.UpdateClubAsync(clubCreated.ClubId, request, _currentUser);
            using (var context = _factory.CreateContext())
            {
                var club = context.Clubs.FirstOrDefault(club => club.Name == request.Name);
                Assert.That(club, Is.Not.Null);
                Assert.That(context.Clubs.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubUpdatedEvent>());
        }


        [Test]
        public async Task UpdateClub_ClubDoesNotExist_DatabaseDoesNotUpdate()
        {
            var request = UpdateClubRequest();

            Assert.ThrowsAsync<QueryException>(async () =>
                await _mutations.UpdateClubAsync(Guid.NewGuid(), request, _currentUser));
            await _publish.Received(0).Publish(Arg.Any<ClubUpdatedEvent>());
        }
        
    }
}
