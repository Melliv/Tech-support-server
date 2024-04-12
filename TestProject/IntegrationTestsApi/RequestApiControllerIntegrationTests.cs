using System.Net;
using DTO.App.V1;
using FluentAssertions;
using Tech_support_server;
using TestProject.Helpers;
using Xunit;

namespace TestProject.IntegrationTestsApi;

public class RequestApiControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private const string RequestCreateUri = "/api/Request";
    private const string RequestUnsolvedUri = "/api/Request/unsolved";
    private const string RequestUpdateUri = "/api/Request/";
    private readonly HttpClient _client;

    public RequestApiControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder => { builder.UseSetting("test_database_name", Guid.NewGuid().ToString()); })
            .CreateClient();
    }


    [Fact]
    public async Task Create_Valid_Request()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now
        };

        await CreateRequest(requestDTO);
    }

    [Fact]
    public async Task Create_Request_With_Missing_Description()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Deadline = DateTime.Now
        };

        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PostAsync(RequestCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Request_With_Missing_Deadline()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description"
        };

        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PostAsync(RequestCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_Unsolved_Requests_Deadline_Expired()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now
        };
        await CreateRequest(requestDTO);

        // ACT
        await GetRequest();
    }

    [Fact]
    public async Task Get_Unsolved_Requests_Deadline_2_Hours_Before_Expiring()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        await CreateRequest(requestDTO);

        // ACT
        var httpResponse = await _client.GetAsync(RequestUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var requestList = await JsonHelper.DeserializeWithWebDefaults<List<Request>>(httpResponse.Content);
        requestList.Should().NotBeNull();
        requestList!.Count.Should().Be(1);
        requestList![0].Description.Should().Be(requestDTO.Description);
        requestList![0].Deadline.Should().Be(requestDTO.Deadline);
    }

    [Fact]
    public async Task Get_Unsolved_Requests_Solved()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2),
            Solved = true
        };
        await CreateRequest(requestDTO);

        // ACT
        var httpResponse = await _client.GetAsync(RequestUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var requestList = await JsonHelper.DeserializeWithWebDefaults<List<Request>>(httpResponse.Content);
        requestList.Should().NotBeNull();
        requestList!.Count.Should().Be(0);
    }

    [Fact]
    public async Task Update_Request_Not_Exist()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Id = Guid.NewGuid(),
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        // await CreateRequest(requestDTO);

        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PutAsync($"{RequestUpdateUri}{requestDTO.Id}", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Request_Id_Dif()
    {
        // // ARRANGE
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        requestDTO = await CreateRequest(requestDTO);
        requestDTO.Description = "New description";

        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PutAsync($"{RequestUpdateUri}fake-id", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_Request_Valid()
    {
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        requestDTO = await CreateRequest(requestDTO);
        requestDTO.Description = "New description";

        await UpdateRequest(requestDTO);
        var requestUpdated = await GetRequest();
        requestUpdated.Description.Should().Be(requestDTO.Description);
        requestUpdated.Deadline.Should().Be(requestDTO.Deadline);
    }

    [Fact]
    public async Task Update_Request_To_Solved()
    {
        var requestDTO = new Request
        {
            Description = "Some description",
            Deadline = DateTime.Now.AddHours(2)
        };
        requestDTO = await CreateRequest(requestDTO);
        requestDTO.Solved = true;

        await UpdateRequest(requestDTO);
        var httpResponse = await _client.GetAsync(RequestUnsolvedUri);
        httpResponse.EnsureSuccessStatusCode();
        var requestList = await JsonHelper.DeserializeWithWebDefaults<List<Request>>(httpResponse.Content);
        requestList.Should().NotBeNull();
        requestList!.Count.Should().Be(0);
    }

    private async Task<Request> CreateRequest(Request requestDTO)
    {
        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PostAsync(RequestCreateUri, data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var obj = await JsonHelper.DeserializeWithWebDefaults<Request>(httpResponse.Content);
        obj.Should().NotBeNull();
        obj!.Id.Should().NotBeEmpty();
        obj!.CreateAt.Should();
        obj!.Description.Should().Be(requestDTO.Description);
        obj!.Solved.Should().Be(requestDTO.Solved);
        obj!.Deadline.Should().Be(requestDTO.Deadline);
        obj!.CreateAt.Should().Be(obj!.UpdatedAt);
        obj!.CreateAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

        return obj;
    }

    private async Task UpdateRequest(Request requestDTO)
    {
        // ACT
        var data = _client.ObjToHttpContent(requestDTO);
        var httpResponse = await _client.PutAsync($"{RequestUpdateUri}{requestDTO.Id}", data);

        // ASSERT
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task<Request> GetRequest()
    {
        var httpResponse = await _client.GetAsync(RequestUnsolvedUri);

        // ASSERT
        httpResponse.EnsureSuccessStatusCode();
        var requestList = await JsonHelper.DeserializeWithWebDefaults<List<Request>>(httpResponse.Content);
        requestList.Should().NotBeNull();
        requestList!.Count.Should().Be(1);
        return requestList[0];
    }
}