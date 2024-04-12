using AutoMapper;
using Contracts.BLL.App;
using DTO.App.V1;
using DTO.App.V1.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebSocket;

namespace Tech_support_server.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class RequestController(IMapper mapper, IAppBLL bll, NotificationHub notificationHub)
    : ControllerBase
{
    private readonly RequestMapper _requestMapper = new(mapper);

    // GET: api/Request
    [HttpGet("unsolved")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Request), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Request>>> GetUnsolvedRequests()
    {
        return Ok((await bll.Request.GetAllUnsolvedAsync()).Select(b => _requestMapper.Map(b)));
    }

    // PUT: api/Request/5
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutRequest(Guid id, Request request)
    {
        var requestBLL = _requestMapper.Map(request);
        if (id != requestBLL.Id) return BadRequest();
        bll.Request.Update(requestBLL);
        await bll.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/Request
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Request), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Request>> PostRequest(Request request)
    {
        var requestBLL = _requestMapper.Map(request);
        if (requestBLL == null) return BadRequest();
        var savedRequest = bll.Request.Add(requestBLL);
        await bll.SaveChangesAsync();
        await notificationHub.SendMessage(NotificationMessage.RequestListUpdated);
        return StatusCode(StatusCodes.Status201Created, savedRequest);
    }
}