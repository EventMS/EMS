using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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
        protected Event latestEvent;
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
            Thread.Sleep(1000);
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

        protected async Task<Mutation> CreateNewUser(string email, string navn)
        {
            lastEmailUsed = email;
            var query = new MutationQueryBuilder().WithCreateUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithId().WithEmail()),
                    new CreateUserRequestInput
                    {
                        BirthDate = "2020-01-01",
                        Name = navn,
                        Email = email,
                        PhoneNumber = "12345678",
                        Password = "Test1234!"
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

        protected async Task<Mutation> LoginOnUser(string email)
        {
            var query = new MutationQueryBuilder().WithLoginUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithId()),
                    new LoginUserRequestInput()
                    {
                        Email = email,
                        Password = "Test1234!"
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

        protected async Task LoginOnAuthorizedClient(string email)
        {
            var result = await LoginOnUser(email);
            _client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse("Bearer " + result.LoginUser.Token);
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

        protected async Task<Mutation> CreateAClub(string navn, List<string> lokaler)
        {
            var name = navn;
            var query = new MutationQueryBuilder().WithCreateClub(
                    new ClubQueryBuilder()
                        .WithClubId().WithDescription(),
                    new CreateClubRequestInput()
                    {
                        Name = name,
                        PhoneNumber = "12345678",
                        Locations = lokaler,
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

            return await CreateSubscription(name);
        }
        protected async Task<Mutation> CreateSubscription(string navn)
        {
            var input = new CreateClubSubscriptionRequestInput()
            {
                Name = navn,
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


        protected async Task CreateMember(string id)
        {
            var result = await _webhookClient.PostAsJsonAsync("webhook/sub", new
            {
                UserId = id,
                ClubSubscriptionId = latestSubscription.ClubSubscriptionId
            });
            result.EnsureSuccessStatusCode();
        }

        protected async Task CreateSignUpToLastCreatedEvent(string id)
        {
            var result = await _webhookClient.PostAsJsonAsync("webhook/event", new
            {
                UserId = id,
                EventId = latestEvent.EventId
            });
            result.EnsureSuccessStatusCode();
        }

        protected async Task CreateMember()
        {
            await CreateMember(lastestUser.Id);
        }


        protected async Task<Mutation> CreateInstructor(string id)
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var query = new MutationQueryBuilder().WithAddInstructor(
                    new PermissionRoleQueryBuilder().WithAllFields()
                    ,
                    latestClub.ClubId,
                    id)
                .Build();
            return await Mutate(query);
        }

        protected async Task<Mutation> CreateInstructor()
        {
            return await CreateInstructor(lastestUser.Id);
        }

        protected async Task<Mutation> CreateEvent()
        {
            var name = Guid.NewGuid().ToString().Substring(15);
            var input = new CreateEventRequestInput
            {
                ClubId = latestClub.ClubId,
                Name = name,
                StartTime = DateTime.Now.ToString(),
                EndTime = DateTime.Now.AddMilliseconds(1).ToString(),
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
            var result = await Mutate(query);
            latestEvent = result.CreateEvent;
            return result;
        }

        protected async Task<Mutation> CreateEvent(string navn, 
            decimal? publicPrice = null,
            string lokale = null, 
            string? instructor = null)
        {
            var name = navn;
            var locations = new List<string>();
            if(lokale != null)
            {
                locations.Add(lokale);
            }

            var instructors = new List<string>();
            if (instructor != null)
            {
                instructors.Add(instructor);
            }

            var input = new CreateEventRequestInput
            {
                ClubId = latestClub.ClubId,
                Name = name,
                StartTime = DateTime.Now.AddHours(1).ToString(),
                EndTime = DateTime.Now.AddHours(2).ToString(),
                Description = "Test klub description",
                Locations = locations,
                EventPrices = new List<EventPriceRequestInput>()
                {
                },
                InstructorForEvents = instructors
            };
            if(publicPrice != null)
            {
                input.PublicPrice = publicPrice;
            }
            if (latestSubscriptions.Count > 0)
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
                    , input)
                .Build();
            var result =  await Mutate(query);
            latestEvent = result.CreateEvent;
            return result;
        }
    }
}