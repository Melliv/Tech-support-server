using System.Net;
using DTO.App.V1;
using FluentAssertions;
using Tech_support_server;
using TestProject.Helpers;
using Xunit;

namespace TestProject.IntegrationTestsApi;

public class TicketApiControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private const string TicketCreateUri = "/api/Ticket";
    private const string TicketUnsolvedUri = "/api/Ticket/unsolved";
    private const string TicketUpdateUri = "/api/Ticket/";
    private readonly HttpClient _client;

    public TicketApiControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder => { builder.UseSetting("test_database_name", Guid.NewGuid().ToString()); })
            .CreateClient();
    }


    [Fact]
    public async Task Create_Valid_Ticket()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now
        };

        await CreateTicket(ticketDTO);
    }

    [Fact]
    public async Task Create_Ticket_With_Missing_Description()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Deadline = DateTime.Now
        };

        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PostAsync(TicketCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Ticket_With_Missing_Deadline()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description"
        };

        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PostAsync(TicketCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_Unsolved_Tickets_Deadline_Expired()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now
        };
        await CreateTicket(ticketDTO);

        // ACT
        await GetTicket();
    }

    [Fact]
    public async Task Get_Unsolved_Tickets_Deadline_2_Hours_Before_Expiring()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        await CreateTicket(ticketDTO);

        // ACT
        var httpResponse = await _client.GetAsync(TicketUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var ticketList = await JsonHelper.DeserializeWithWebDefaults<List<Ticket>>(httpResponse.Content);
        ticketList.Should().NotBeNull();
        ticketList!.Count.Should().Be(1);
        ticketList![0].Description.Should().Be(ticketDTO.Description);
        ticketList![0].Deadline.Should().Be(ticketDTO.Deadline);
    }

    [Fact]
    public async Task Get_Unsolved_Tickets_Solved()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2),
            Solved = true
        };
        await CreateTicket(ticketDTO);

        // ACT
        var httpResponse = await _client.GetAsync(TicketUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var ticketList = await JsonHelper.DeserializeWithWebDefaults<List<Ticket>>(httpResponse.Content);
        ticketList.Should().NotBeNull();
        ticketList!.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Get_Unsolved_Tickets_In_Right_Order()
    {
        // // ARRANGE
        var ticketDTO1 = new Ticket
        {
            Description = "first description",
            Deadline = DateTime.Now,
        };
        var ticketDTO2 = new Ticket
        {
            Description = "second description",
            Deadline = DateTime.Now.AddHours(1),
        };
        var ticketDTO3 = new Ticket
        {
            Description = "third description",
            Deadline = DateTime.Now.AddDays(1),
        };
        await CreateTicket(ticketDTO3);
        await CreateTicket(ticketDTO1);
        await CreateTicket(ticketDTO2);

        // ACT
        var httpResponse = await _client.GetAsync(TicketUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var ticketList = await JsonHelper.DeserializeWithWebDefaults<List<Ticket>>(httpResponse.Content);
        ticketList.Should().NotBeNull();
        ticketList!.Count.Should().Be(3);
        ticketList[0].Description.Should().Be(ticketDTO1.Description);
        ticketList[1].Description.Should().Be(ticketDTO2.Description);
        ticketList[2].Description.Should().Be(ticketDTO3.Description);
    }

    [Fact]
    public async Task Update_Ticket_Not_Exist()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Id = Guid.NewGuid(),
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        // await CreateTicket(ticketDTO);

        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PutAsync($"{TicketUpdateUri}{ticketDTO.Id}", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Ticket_Id_Dif()
    {
        // // ARRANGE
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        ticketDTO = await CreateTicket(ticketDTO);
        ticketDTO.Description = "New description";

        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PutAsync($"{TicketUpdateUri}fake-id", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Ticket_Valid()
    {
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        ticketDTO = await CreateTicket(ticketDTO);
        ticketDTO.Description = "New description";

        await UpdateTicket(ticketDTO);
        var ticketUpdated = await GetTicket();
        ticketUpdated.Description.Should().Be(ticketDTO.Description);
        ticketUpdated.Deadline.Should().Be(ticketDTO.Deadline);
    }

    [Fact]
    public async Task Update_Ticket_To_Solved()
    {
        var ticketDTO = new Ticket
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        ticketDTO = await CreateTicket(ticketDTO);
        ticketDTO.Solved = true;

        await UpdateTicket(ticketDTO);
        var httpResponse = await _client.GetAsync(TicketUnsolvedUri);
        httpResponse.EnsureSuccessStatusCode();
        var ticketList = await JsonHelper.DeserializeWithWebDefaults<List<Ticket>>(httpResponse.Content);
        ticketList.Should().NotBeNull();
        ticketList!.Count.Should().Be(0);
    }

    private async Task<Ticket> CreateTicket(Ticket ticketDTO)
    {
        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PostAsync(TicketCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var obj = await JsonHelper.DeserializeWithWebDefaults<Ticket>(httpResponse.Content);
        obj.Should().NotBeNull();
        obj!.Id.Should().NotBeEmpty();
        obj!.CreateAt.Should();
        obj!.Description.Should().Be(ticketDTO.Description);
        obj!.Solved.Should().Be(ticketDTO.Solved);
        obj!.Deadline.Should().Be(ticketDTO.Deadline);
        obj!.CreateAt.Should().Be(obj!.UpdatedAt);
        obj!.CreateAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

        return obj;
    }

    private async Task UpdateTicket(Ticket ticketDTO)
    {
        // ACT
        var data = _client.ObjToHttpContent(ticketDTO);
        var httpResponse = await _client.PutAsync($"{TicketUpdateUri}{ticketDTO.Id}", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<Ticket> GetTicket()
    {
        var httpResponse = await _client.GetAsync(TicketUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var ticketList = await JsonHelper.DeserializeWithWebDefaults<List<Ticket>>(httpResponse.Content);
        ticketList.Should().NotBeNull();
        ticketList!.Count.Should().Be(1);
        return ticketList[0];
    }
}