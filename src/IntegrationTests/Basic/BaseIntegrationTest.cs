using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;
using Test;

namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    public class BaseIntegrationTest
    {
        protected HttpClient _client;
        protected HttpClient _webhookClient;
        protected string lastEmailUsed;
        protected Club latestClub;
        protected IdentityApplicationUser lastestUser;
        protected List<ClubSubscription> latestSubscriptions = new List<ClubSubscription>();

        public ClubSubscription latestSubscription => latestSubscriptions?.FirstOrDefault();

        [SetUp]
        public void SetUp()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5102/");
            _webhookClient = new HttpClient();
            _webhookClient.BaseAddress = new Uri("http://localhost:5112/");
            lastEmailUsed = null;
        }
        protected async Task<Query> Query(string query)
        {
            var request = new Request()
            {
                query = query
            };
            var result = await _client.PostAsJsonAsync("/", request);
            result.EnsureSuccessStatusCode();
            var s = await result.Content.ReadAsStringAsync();
            if (s.StartsWith("{\"errors"))
            {
                throw new Exception(s);
            }
            var r = await result.Content.ReadAsAsync<QueryResponse>();
            return r.data;
        }

        protected async Task<Mutation> Mutate(string query)
        {
            var request = new Request()
            {
                query = query
            };
            var result = await _client.PostAsJsonAsync("/", request);
            result.EnsureSuccessStatusCode();

            var s = await result.Content.ReadAsStringAsync();
            if (s.StartsWith("{\"errors"))
            {
                throw new Exception(s);
            }
            var r = await result.Content.ReadAsAsync<MutationResponse>();
            return r.data;
        }

        protected async Task<Mutation> CreateNewUser()
        {
            lastEmailUsed = Guid.NewGuid() + "@test.com";
            var query = new MutationQueryBuilder().WithCreateUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithId().WithEmail()),
                    new CreateUserRequestInput
                    {
                        BirthDate = "2020-01-01",
                        Name = "Mads",
                        Email = lastEmailUsed,
                        PhoneNumber = "12345678",
                        Password = "Pass123!"
                    })
                .Build();

            var result = await Mutate(query);
            lastestUser = result.CreateUser.User;
            return result;
        }

        protected async Task<Mutation> LoginOnUser()
        {

            var query = new MutationQueryBuilder().WithLoginUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithAllFields()),
                    new LoginUserRequestInput()
                    {
                        Email = lastEmailUsed,
                        Password = "Pass123!"
                    })
                .Build();
            return await Mutate(query);
        }

        protected async Task CreateAuthorizedClient()
        {
            var result = await CreateNewUser();
            _client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + result.CreateUser.Token);
        }


        protected async Task<Mutation> CreateAClub()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithCreateClub(
                    new ClubQueryBuilder()
                        .WithClubId().WithDescription(),
                    new CreateClubRequestInput()
                    {
                        Name = name,
                        PhoneNumber = "12345678",
                        Locations = new List<string>()
                        {
                            "Test1",
                            "Test2"
                        },
                        Description = "Test club",
                        RegistrationNumber = "1234",
                        Address = "Test klub address",
                        AccountNumber = "12345678"
                    })
                .Build();

            var result = await Mutate(query);
            latestClub = result.CreateClub;

            return result;
        }
        
        protected async Task<Mutation> CreateSubscription()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var input = new CreateClubSubscriptionRequestInput()
            {
                Name = name,
                Price = 100,
                ClubId = latestClub.ClubId
            };
            if (latestSubscription?.ClubSubscriptionId != null)
            {
                input.ReferenceId = latestSubscription.ClubSubscriptionId;
            }

            var query = new MutationQueryBuilder().WithCreateClubSubscription(
                    new ClubSubscriptionQueryBuilder().WithClubSubscriptionId()
                        ,
                    input)
                .Build();
            var result = await Mutate(query);
            latestSubscriptions.Add(result.CreateClubSubscription);

            return result;
        }

        protected async Task CreateMember()
        {
            var result = await _webhookClient.PostAsJsonAsync("webhook/sub", new
            {
                UserId = lastestUser.Id,
                ClubSubscriptionId = latestSubscription.ClubSubscriptionId
            });
            result.EnsureSuccessStatusCode();
        }

        protected async Task<Mutation> CreateInstructor()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithAddInstructor(
                    new PermissionRoleQueryBuilder().WithAllFields()
                    ,
                    latestClub.ClubId,
                    lastestUser.Id)
                .Build();
            return await Mutate(query);
        }

        protected async Task<Mutation> CreateEvent()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var input = new CreateEventRequestInput
            {
                ClubId = latestClub.ClubId,
                Name = name,
                StartTime = "2021-01-01",
                EndTime = "2021-01-02",
                Description = "Test klub description",
                PublicPrice = 10,
                Locations = new List<string>()
                {

                },
                EventPrices = new List<EventPriceRequestInput>()
                {
                }
            };
            if(latestSubscriptions.Count > 0)
            {
                input.EventPrices = latestSubscriptions
                    .Select(sub => new EventPriceRequestInput()
                    {
                        ClubSubscriptionId = sub.ClubSubscriptionId,
                        Price = 50
                    }).ToList();
            }

            var query = new MutationQueryBuilder().WithCreateEvent(
                    new EventQueryBuilder().WithEventId()
                    ,input )
                .Build();
            return await Mutate(query);
        }
    }
}