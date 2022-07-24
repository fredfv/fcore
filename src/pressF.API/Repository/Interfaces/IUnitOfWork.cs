using System;
using System.Threading.Tasks;

namespace pressF.API.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}