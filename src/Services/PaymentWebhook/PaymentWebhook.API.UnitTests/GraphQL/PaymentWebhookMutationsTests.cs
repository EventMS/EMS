using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.SharedTesting.Helper;
using NSubstitute;
using NUnit.Framework;

using EMS.PaymentWebhook_Services.API.Context;
using EMS.PaymentWebhook_Services.API.Controllers.Request;
using EMS.PaymentWebhook_Services.API.GraphQlQueries;
using EMS.PaymentWebhook_Services.API.Mapper;
using Test;

namespace EMS.PaymentWebhook_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class PaymentWebhookMutationsTests : BaseMutationsSetupTests<PaymentWebhookContext>
    {
        
        #region Setup
        private PaymentWebhookMutations _mutations;

        [SetUp]
        public void SetUp()
        {
            var mapper = CreateMapper();
            _mutations = new PaymentWebhookMutations(_context, _eventService, mapper, _authorizationService);

        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<PaymentWebhookProfile>();
            });
            return new AutoMapper.Mapper(config);
        }
        #endregion


        [Test]
        public async Task CreatePaymentWebhook_ValidRequest_AddedToDatabase()
        {
            var request = new CreatePaymentWebhookRequest()
            {
                Name = "Te"
            };

            await _mutations.CreatePaymentWebhookAsync(request);

            using (var context = _factory.CreateContext())
            {
                var template1 = context.PaymentWebhooks.FirstOrDefault(template1 => template1.Name == request.Name);
                Assert.That(template1, Is.Not.Null);
                Assert.That(context.PaymentWebhooks.Count(), Is.EqualTo(1));
            }

            await _publish.Received(1).Publish(Arg.Any<PaymentWebhookCreatedEvent>());
        }

        [Test]
        public async Task CreatePaymentWebhook_InvalidRequest_DatabaseFails()
        {
            var request = new CreatePaymentWebhookRequest()
            {
                Name = "Test"
            };

            Assert.ThrowsAsync<ValidationException>(async () => await _mutations.CreatePaymentWebhookAsync(request));
            await _publish.Received(0).Publish(Arg.Any<PaymentWebhookCreatedEvent>());
        }



        [Test]
        public async Task CreateTemplate2_InvalidRequest_DatabaseFails()
        {
            var test = new QueryQueryBuilder()
                .WithClubs(new ClubQueryBuilder().WithAllFields());
        }

    }
}
