using System;
using System.Collections.Generic;
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
        protected string currentEmail;
        protected Guid currentClub;
        [SetUp]
        public void SetUp()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5102/");
            currentEmail = null;
            currentClub = Guid.Empty;
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
            currentEmail = Guid.NewGuid() + "@test.com";
            var query = new MutationQueryBuilder().WithCreateUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithAllFields()),
                    new CreateUserRequestInput
                    {
                        BirthDate = "2020-01-01",
                        Name = "Mads",
                        Email = currentEmail,
                        PhoneNumber = "12345678",
                        Password = "Pass123!"
                    })
                .Build();

            return await Mutate(query);
        }

        protected async Task<Mutation> LoginOnUser()
        {

            var query = new MutationQueryBuilder().WithLoginUser(
                    new IdentityResponseQueryBuilder()
                        .WithToken()
                        .WithUser(new IdentityApplicationUserQueryBuilder().WithAllFields()),
                    new LoginUserRequestInput()
                    {
                        Email = currentEmail,
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
                        .WithAllFields()
                        .ExceptValidate(),
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
            return await Mutate(query);
        }

        protected async Task ArrangeClub()
        {
            await CreateAuthorizedClient();
            var result = await CreateAClub();
            currentClub = new Guid(result.CreateClub.ClubId.ToString());
        }
    }
}