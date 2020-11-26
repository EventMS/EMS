using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Test;

namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    public class AcceptTestRequests : BaseIntegrationTest
    {
        [Test]
        public async Task GenerateAcceptTestData()
        {
            var JS = await CreateNewUser("zumba@test.com", "Jesper Strøm");
            var SWB = await CreateNewUser("test@test.com", "Simon Wessmann Brok");
            var MSN = await CreateNewUser("golf@gokart.com", "Mads Skytte Nilesen");
            await CreateNewUser("tester@bruger.com", "Tester Bruger");

            await LoginOnAuthorizedClient("zumba@test.com");
            await CreateAClub("Tante Olgas Zumba Tøzer", new List<string>{
                "Rumba rummet", "Spirituel zone" });
            await CreateSubscription("Super Zumba Tøzz");
            await CreateMember(SWB.CreateUser.User.Id);
            Thread.Sleep(1000);
            await CreateInstructor(SWB.CreateUser.User.Id);
            //Get room ids
            var clubRooms = await Query(new QueryQueryBuilder().WithClubById(new ClubQueryBuilder()
                .WithClubId().WithRooms(new RoomQueryBuilder()
                .WithName()
                .WithRoomId()),
                latestClub.ClubId).Build());
            var rooms = new List<Room>(clubRooms.ClubById.Rooms);
            await CreateEvent("Zumba 1", null,
                rooms.Find(room => room.Name.Equals("Spirituel zone")).RoomId,
                SWB.CreateUser.User.Id); //Need room ids "Spirituel Zone"
            await CreateSignUpToLastCreatedEvent(JS.CreateUser.User.Id);
            await CreateSignUpToLastCreatedEvent(SWB.CreateUser.User.Id);
            await CreateEvent("Zumba 2", 30, null, SWB.CreateUser.User.Id);
            await CreateSignUpToLastCreatedEvent(MSN.CreateUser.User.Id);

            await LoginOnAuthorizedClient("test@test.com");
            await CreateAClub("Den mørke skov", new List<string>{
                "Gandalfs gemakker", 
                "Legolas’ elverhule",
                "Gimlis grotte"
            });
            latestSubscriptions = new List<ClubSubscription>();
            await CreateSubscription("Højelver");
            await CreateSubscription("Trold");
            await CreateSubscription("Troldmand");
            await CreateMember(JS.CreateUser.User.Id);
            await CreateMember(MSN.CreateUser.User.Id);
            await CreateEvent("Bue og pil", null);
            await CreateSignUpToLastCreatedEvent(MSN.CreateUser.User.Id);
            await CreateEvent("Tryllestøv", 30);
            await CreateSignUpToLastCreatedEvent(JS.CreateUser.User.Id);


            await LoginOnAuthorizedClient("golf@gokart.com");
            await CreateAClub("Golf og Gokart", new List<string>{
                "Gokart mesterskabsbane",
                "Driving rangen"
            });
            latestSubscriptions = new List<ClubSubscription>();
            await CreateSubscription("Hele pakken");
            await CreateSubscription("Golferen");
            await CreateSubscription("Zoomeren");
            await CreateMember(JS.CreateUser.User.Id);
            Thread.Sleep(1000);
            await CreateInstructor(JS.CreateUser.User.Id);
            await CreateMember(SWB.CreateUser.User.Id);
            await CreateEvent("Wroom", null, null, JS.CreateUser.User.Id);
            await CreateSignUpToLastCreatedEvent(SWB.CreateUser.User.Id);
            await CreateEvent("Fore helved", 30, null, JS.CreateUser.User.Id);
            await CreateSignUpToLastCreatedEvent(MSN.CreateUser.User.Id);
        }
    }
}