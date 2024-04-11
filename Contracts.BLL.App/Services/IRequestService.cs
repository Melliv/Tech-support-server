using BLL.App.DTO;
using Contract.DAL.App.Repositories;
using Contracts.BLL.Base.Services;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services;

public interface IRequestService : IBaseEntityService<Request, DALAppDTO.Request>, IRequestRepositoryCustom<Request>
{
    new Task<IEnumerable<Request>> GetAllSolvedAsync();
}