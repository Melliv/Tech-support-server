using AutoMapper;
using Contract.DAL.App;
using Contract.DAL.App.Repositories;
using DAL.APP.EF.Repositories;
using DAL.Base.EF;

namespace DAL.APP.EF;

public class AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper)
    : BaseUnitOfWork<AppDbContext>(uowDbContext), IAppUnitOfWork
{
    public ITicketRepository Ticket =>
        GetRepository(() => new TicketRepository(UowDbContext, mapper));
}