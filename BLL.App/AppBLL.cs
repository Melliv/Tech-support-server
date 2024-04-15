using AutoMapper;
using BLL.App.Services;
using BLL.Base;
using Contract.DAL.App;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;

namespace BLL.App;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    protected IMapper Mapper;

    public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
    {
        Mapper = mapper;
    }

    public ITicketService Ticket =>
        GetService<ITicketService>(() => new TicketService(Uow, Uow.Ticket, Mapper));
}