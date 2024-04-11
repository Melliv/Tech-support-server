using AutoMapper;
using Contract.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.APP.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.APP.EF.Repositories;

public class RequestRepository(AppDbContext dbContext, IMapper mapper)
    : BaseRepository<Request, Domain.App.Request, AppDbContext>(dbContext, new RequestMapper(mapper)),
        IRequestRepository
{
    public async Task<IEnumerable<Request>> GetAllSolvedAsync()
    {
        var query = CreateQuery();
        var resQuery = query
            .Where(r => r.Deadline.AddHours(-1) > DateTime.Now && r.Solved == false);

        return await resQuery.Select(x => Mapper.Map(x)).ToListAsync();
    }
}