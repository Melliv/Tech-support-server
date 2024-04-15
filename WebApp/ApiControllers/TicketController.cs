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
public class TicketController(IMapper mapper, IAppBLL bll, NotificationHub notificationHub)
    : ControllerBase
{
    private readonly TicketMapper _ticketMapper = new(mapper);

    // GET: api/Ticket
    [HttpGet("unsolved")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Ticket), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetUnsolvedTickets()
    {
        return Ok((await bll.Ticket.GetAllUnsolvedAsync()).Select(b => _ticketMapper.Map(b)));
    }

    // PUT: api/Ticket/5
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutTicket(Guid id, Ticket ticket)
    {
        var ticketBLL = _ticketMapper.Map(ticket);
        if (id != ticketBLL.Id) return BadRequest();
        bll.Ticket.Update(ticketBLL);
        await bll.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/Ticket
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Ticket), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ticket>> PostRequest(Ticket ticket)
    {
        var ticketBLL = _ticketMapper.Map(ticket);
        if (ticketBLL == null) return BadRequest();
        var savedTicket = bll.Ticket.Add(ticketBLL);
        await bll.SaveChangesAsync();
        await notificationHub.SendMessage(NotificationMessage.TicketListUpdated);
        return StatusCode(StatusCodes.Status201Created, savedTicket);
    }
}