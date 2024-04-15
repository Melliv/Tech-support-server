using Contracts.BLL.App.Services;
using Contracts.BLL.Base;

namespace Contracts.BLL.App;

public interface IAppBLL : IBaseBLL
{
    ITicketService Ticket { get; }
}