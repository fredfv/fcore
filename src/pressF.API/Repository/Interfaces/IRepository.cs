using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pressF.API.Repository.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
       public void Add(TEntity obj);
       public Task<TEntity> GetById(string id);
       public Task<IEnumerable<TEntity>> GetAll();
       public void Update(TEntity obj);
       public void Remove(string id);
    }
}