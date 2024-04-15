using AutoMapper;
using Contract.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories;

public class TicketRepository(AppDbContext dbContext, IMapper mapper)
    : BaseRepository<Ticket, Domain.App.Ticket, AppDbContext>(dbContext, new TicketMapper(mapper)),
        ITicketRepository
{
    public async Task<IEnumerable<Ticket>> GetAllUnsolvedAsync()
    {
        var query = CreateQuery();
        var resQuery = query
            .Where(r => r.Solved == false).OrderBy(r => r.Deadline);

        return await resQuery.Select(x => Mapper.Map(x)).ToListAsync();
    }
}