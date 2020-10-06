using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Controllers.Request;
using EMS.ClubMember_Services.API.GraphQlQueries;
using EMS.ClubMember_Services.API.Mapper;

namespace EMS.ClubMember_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class ClubMemberMutationsTests : BaseMutationsSetupTests<ClubMemberContext>
    {
        
        #region Setup
        private ClubMemberMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new ClubMemberMutations(_context, _eventService, mapper);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ClubMemberProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion

        /*
        [Test]
        public async Task CreateClubMember_ValidRequest_AddedToDatabase()
        {
            var request = new CreateClubMemberRequest()
            {
                Name = "Te"
            };

            await _mutations.CreateClubMemberAsync(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.ClubMembers.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.ClubMembers.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<ClubMemberCreatedEvent>());
        }

        [Test]
        public async Task CreateClubMember_InvalidRequest_DatabaseFails()
        {
            var request = new CreateClubMemberRequest()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreateClubMemberAsync(request));
            await _publish.Received(0).Publish(Arg.Any<ClubMemberCreatedEvent>());
        }
        */

    }
}
