using Contract.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contract.DAL.App;

public interface IAppUnitOfWork: IBaseUnitOfWork
{
    IRequestRepository Request { get; }
}